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
        private DatabaseAccess dbAccess;

        public DatabaseOrderDummyFiller()
        {
            dbAccess = new DatabaseAccess("server=localhost;port=3306;database=ordertest;user=root;password=root");
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
            
            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateNextOrder);
            dbAccess.CloseConnection();
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertNextOrder = "INSERT INTO `nextorder` (`id`, `storeId`, `productId`, `amount`) VALUES ('1', '1', '1', '10');";
            
            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Insert(mySqlConnection, queryInsertNextOrder);
            dbAccess.CloseConnection();
        }
    }
}
