using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.StockAPI
{
    class DatabaseStockDummyFiller
    {
        private DatabaseAccess dbAccess;

        public DatabaseStockDummyFiller()
        {
            dbAccess = new DatabaseAccess("server=localhost;port=3306;database=stocktest;user=root;password=root");
        }

        public static async Task EmptyAndFillDatabases()
        {
            DatabaseStockDummyFiller dbFiller = new DatabaseStockDummyFiller();
            await dbFiller.ClearDatabaseTestRecords();
            await dbFiller.FillDatabaseTablesWithDummyData();
        }

        private async Task ClearDatabaseTestRecords()
        {
            string queryTruncateStock = "truncate `stock`;";
            string queryTruncateStockMutation = "truncate `stockmutation`;";

            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateStock);
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateStockMutation);
            dbAccess.CloseConnection();
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertStock = "INSERT INTO `stock` (`id`, `storeId`, `productId`, `amount`, `date`) VALUES ('1', '1', '1', '10', DATE_SUB(NOW(), INTERVAL 10 DAY));";
            string queryInsertStockMutation1 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('1', '1', '1', '-2', 'Sold', subtime(now(), '9 00:00:00'));";
            string queryInsertStockMutation2 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('2', '1', '1', '-2', 'Sold', subtime(now(), '8 00:00:00'));";
            string queryInsertStockMutation3 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('3', '1', '1', '-3', 'Broken', subtime(now(), '7 00:00:00'));";
            string queryInsertStockMutation4 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('4', '1', '1', '10', 'Bought', subtime(now(), '6 00:00:00'));";
            string queryInsertStockMutation5 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('5', '1', '1', '-4', 'Sold', subtime(now(), '4 00:00:00'));";
            
            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Insert(mySqlConnection, queryInsertStock);
            await QueryExecutor.Insert(mySqlConnection, queryInsertStockMutation1);
            await QueryExecutor.Insert(mySqlConnection, queryInsertStockMutation2);
            await QueryExecutor.Insert(mySqlConnection, queryInsertStockMutation3);
            await QueryExecutor.Insert(mySqlConnection, queryInsertStockMutation4);
            await QueryExecutor.Insert(mySqlConnection, queryInsertStockMutation5);
            dbAccess.CloseConnection();
        }
    }
}
