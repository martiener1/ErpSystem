using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.OrderAPI
{
    class DatabaseOrderDummyFiller
    {
        string connectionString;

        public DatabaseOrderDummyFiller()
        {
            connectionString = DatabaseConnectionString.GetAzureConnectionString("ordertest");
        }

        public static async Task EmptyAndFillDatabases()
        {
            DatabaseOrderDummyFiller dbFiller = new DatabaseOrderDummyFiller();
            await dbFiller.ClearDatabaseTestRecords();
            await dbFiller.FillDatabaseTablesWithDummyData();
        }

        private async Task ClearDatabaseTestRecords()
        {
            string queryTruncateNextOrder = "truncate `nextorder`;";
            
            await QueryExecutor.Truncate(connectionString, queryTruncateNextOrder);
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertNextOrder = "INSERT INTO `nextorder` (`id`, `storeId`, `productId`, `amount`) VALUES ('1', '1', '1', '10');";
            
            await QueryExecutor.Insert(connectionString, queryInsertNextOrder);
        }
    }
}
