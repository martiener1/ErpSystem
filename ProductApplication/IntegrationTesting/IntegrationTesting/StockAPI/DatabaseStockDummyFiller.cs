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
        string connectionString;

        public DatabaseStockDummyFiller()
        {
            connectionString = DatabaseConnectionString.GetAzureConnectionString("stocktest");
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
            
            await QueryExecutor.Truncate(connectionString, queryTruncateStock);
            await QueryExecutor.Truncate(connectionString, queryTruncateStockMutation);
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertStock = "INSERT INTO `stock` (`id`, `storeId`, `productId`, `amount`, `date`) VALUES ('1', '1', '1', '10', DATE_SUB(NOW(), INTERVAL 10 DAY));";
            string queryInsertStockMutation1 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('1', '1', '1', '-2', 'Sold', subtime(now(), '9 00:00:00'));";
            string queryInsertStockMutation2 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('2', '1', '1', '-2', 'Sold', subtime(now(), '8 00:00:00'));";
            string queryInsertStockMutation3 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('3', '1', '1', '-3', 'Broken', subtime(now(), '7 00:00:00'));";
            string queryInsertStockMutation4 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('4', '1', '1', '10', 'Bought', subtime(now(), '6 00:00:00'));";
            string queryInsertStockMutation5 = "INSERT INTO `stockmutation` (`id`, `storeId`, `productId`, `amount`, `reason`, `datetime`) VALUES ('5', '1', '1', '-4', 'Sold', subtime(now(), '4 00:00:00'));";
            
            await QueryExecutor.Insert(connectionString, queryInsertStock);
            await QueryExecutor.Insert(connectionString, queryInsertStockMutation1);
            await QueryExecutor.Insert(connectionString, queryInsertStockMutation2);
            await QueryExecutor.Insert(connectionString, queryInsertStockMutation3);
            await QueryExecutor.Insert(connectionString, queryInsertStockMutation4);
            await QueryExecutor.Insert(connectionString, queryInsertStockMutation5);
        }
    }
}
