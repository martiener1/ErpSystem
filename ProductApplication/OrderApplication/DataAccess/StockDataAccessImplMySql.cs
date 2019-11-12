using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shared.Data;
using Shared.Models;
using StockAPI.LocalModels;

namespace StockAPI.DataAccess
{
    public class StockDataAccessImplMySql : StockDataAccess
    {
        string dbConnectionString;

        public StockDataAccessImplMySql(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public StockDataAccessImplMySql()
        {
            this.dbConnectionString = "server=localhost;port=3306;database=stock;user=root;password=root";
        }

        private MySqlConnection NewConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            dbCon.Connect(dbConnectionString);
            return dbCon.Connection;
        }

        private void CloseConnection()
        {
            DBConnection.Instance().Close();
        }

        public async Task AddMutation(int storeId, StockMutation stockMutation)
        {
            string query = "INSERT INTO `stockmutation` (`storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES (@0, @1, @2, @3, @4);";
            await QueryExecutor.Insert(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, stockMutation.productId, MySqlDbType.Int32, stockMutation.amount, MySqlDbType.VarChar, stockMutation.reason, MySqlDbType.VarChar, stockMutation.dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            CloseConnection();
        }

        public async Task AddMutationBulk(int storeId, StockMutation[] stockMutations)
        {
            // THIS SHOULD BE DONE DIFFERENTLY
            // This is just so it works, but it can be way more efficient if it's done all in one argument.
            foreach (StockMutation mutation in stockMutations)
            {
                await AddMutation(storeId, mutation);
            }
        }

        public async Task<int> GetCurrentStock(int storeId, long productId)
        {
            return await GetStock(storeId, productId, DateTime.Now);
        }

        public async Task<StockMutation[]> GetMutationHistory(int selectedStoreId, long selectProductId, int timeSpanInDays, DateTime startingDate)
        {
            string query = "select * from stockmutation where storeId = @0 and productId = @1 and `datetime` > @3 and `datetime` < date_add(@3, interval @2 day);";
            object[][] result = await QueryExecutor.SelectMultiple(NewConnection(), query, MySqlDbType.Int32, selectedStoreId, MySqlDbType.Int32, selectProductId, MySqlDbType.Int32, timeSpanInDays, MySqlDbType.DateTime, startingDate);
            CloseConnection();

            StockMutation[] mutations = new StockMutation[result.Length];
            int counter = 0;
            foreach (object[] row in result)
            {
                int mutationId = (int)row[0];
                int storeId = (int)row[1];
                int productId = (int)row[2];
                int amount = (int)row[3];
                string reasonString = (string)row[4];
                MutationReason reason = Enum.Parse<MutationReason>(reasonString);
                DateTime dateTime = (DateTime)row[5];

                StockMutation mutation = new StockMutation(mutationId, storeId, productId, amount, reason, dateTime);
                mutations[counter] = mutation;
                counter++;
            }
            if (counter > 0)
            {
                return mutations;
            } else
            {
                return null;
            }
        }

        public async Task<int[]> GetStockHistory(int storeId, long productId, int timeSpanInDays, DateTime startingDate)
        {
            int[] stockHistory = new int[timeSpanInDays];
            for (int i = 0 ; i < timeSpanInDays; i++)
            {
                stockHistory[i] = await GetStock(storeId, productId, startingDate.AddDays(i));
            }
            return stockHistory;
        }

        private async Task<int> CalculateCurrentStock(int storeId, long productId)
        {
            return await CalculateStock(storeId, productId, DateTime.Now);
        }

        private async Task<int> GetStock(int storeId, long productId, DateTime dateTime)
        {
            string query = "select amount from stock where storeId = @0 and productId = @1 and `date` = @2;";
            object[] result = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId, MySqlDbType.Date, dateTime);
            CloseConnection();

            if (result[0] == null)
            {
                return await CalculateStock(storeId, productId, dateTime);
            }
            else return (int)result[0];
        }

        private async Task<int> CalculateStock(int storeId, long productId, DateTime date)
        {
            string query = "select `date`, `amount` from stock where `date` in (select max(`date`) from stock where storeId = @0 and productId = @1 and `date` < @2);";
            object[] result = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId, MySqlDbType.Date, date);
            CloseConnection();
            int startingAmount;
            DateTime startingDateTime;
            if (result[0] == null)
            {
                startingAmount = 0;
                startingDateTime = DateTime.MinValue;
            }
            else
            {
                startingDateTime = (DateTime)result[0];
                startingAmount = (int)result[1];
            }

            StockMutation[] mutations = await GetAllMutationsBetweenDates(storeId, productId, startingDateTime, date.Date);

            int stockAmount = startingAmount;
            if (mutations != null)
            {
                foreach (StockMutation m in mutations)
                {
                    stockAmount += m.amount;
                }
            }

            await InsertStockAmount(storeId, productId, stockAmount, date);

            return stockAmount;
        }

        private async Task<StockMutation[]> GetAllMutationsBetweenDates(int selectStoreId, long selectProductId, DateTime startingDate, DateTime endingDate)
        {
            string query = "select * from stockmutation where storeId = @0 and productId = @1 and `datetime` > @2 and `datetime` < @3;";
            object[][] result = await QueryExecutor.SelectMultiple(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.Int32, selectProductId, MySqlDbType.DateTime, startingDate, MySqlDbType.DateTime, endingDate);
            CloseConnection();
            StockMutation[] mutations = new StockMutation[result.Length];
            int counter = 0;
            foreach (object[] row in result)
            {
                int mutationId = (int)row[0];
                int storeId = (int)row[1];
                int productId = (int)row[2];
                int amount = (int)row[3];
                string reasonString = (string)row[4];
                MutationReason reason = Enum.Parse<MutationReason>(reasonString);
                DateTime dateTime = (DateTime)row[5];

                StockMutation mutation = new StockMutation(mutationId, storeId, productId, amount, reason, dateTime);
                mutations[counter] = mutation;
                counter++;
            }
            if (counter > 0)
            {
                return mutations;
            }
            else
            {
                return null;
            }
        }

        private async Task InsertStockAmount(int storeId, long productId, int amount, DateTime date)
        {
            string query = "INSERT INTO `stock` (`storeId`, `productId`, `amount`, `date`) VALUES (@0, @1, @2, @3);";
            await QueryExecutor.Insert(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId, MySqlDbType.Int32, amount, MySqlDbType.DateTime, date);
            CloseConnection();
        }




    }
}
