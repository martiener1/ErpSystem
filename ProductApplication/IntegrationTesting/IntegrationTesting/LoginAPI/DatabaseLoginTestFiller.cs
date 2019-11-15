using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    class DatabaseLoginTestFiller
    {
        private DatabaseAccess dbAccess;

        public DatabaseLoginTestFiller()
        {
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("logintest");
            dbAccess = new DatabaseAccess(connectionString);
        }

        public static async Task EmptyAndFillDatabases()
        {
            DatabaseLoginTestFiller dbFiller = new DatabaseLoginTestFiller();
            await dbFiller.ClearDatabaseTestRecords();
            await dbFiller.FillDatabaseTablesWithDummyData();
        }

        private async Task ClearDatabaseTestRecords()
        {
            string queryTruncateTokens = "truncate tokens;";
            string queryTruncateUsers = "truncate users;";
            string queryTruncateStores = "truncate stores;";

            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Delete(mySqlConnection, queryTruncateTokens);
            await QueryExecutor.Delete(mySqlConnection, queryTruncateUsers);
            await QueryExecutor.Delete(mySqlConnection, queryTruncateStores);
            dbAccess.CloseConnection();
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertStore = "INSERT INTO `stores` (`id`, `name`, `city`, `address`) VALUES ('1', 'store1', 'cityStore1', 'addressStore1');";
            string queryInsertUser1 = "INSERT INTO `users` (`id`, `storeid`, `loginname`, `password`, `firstname`, `lastname`, `birthdate`) VALUES('1', '1', 'user1', 'password', 'user1', 'user1', '2000-01-01');";
            string queryInsertUser2 = "INSERT INTO `users` (`id`, `storeid`, `loginname`, `password`, `firstname`, `lastname`, `birthdate`) VALUES('2', '1', 'user2', 'password', 'user2', 'user2', '2000-01-01');";
            string queryInsertUser3 = "INSERT INTO `users` (`id`, `storeid`, `loginname`, `password`, `firstname`, `lastname`, `birthdate`) VALUES('3', '1', 'user3', 'password', 'user3', 'user3', '2000-01-01');";
            string queryInsertToken1 = "INSERT INTO `tokens` (`id`, `userid`, `token`, `expirationdate`) VALUES ('1', '1', 'token1', ADDTIME(now(), '00:30:00'));";
            string queryInsertToken2 = "INSERT INTO `tokens` (`id`, `userid`, `token`, `expirationdate`) VALUES ('2', '2', 'token2', ADDTIME(now(), '00:30:00'));";
            string queryInsertToken3 = "INSERT INTO `tokens` (`id`, `userid`, `token`, `expirationdate`) VALUES ('3', '3', 'token3', SUBTIME(now(), '00:30:00'));";

            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Insert(mySqlConnection, queryInsertStore);
            await QueryExecutor.Insert(mySqlConnection, queryInsertUser1);
            await QueryExecutor.Insert(mySqlConnection, queryInsertUser2);
            await QueryExecutor.Insert(mySqlConnection, queryInsertUser3);
            await QueryExecutor.Insert(mySqlConnection, queryInsertToken1);
            await QueryExecutor.Insert(mySqlConnection, queryInsertToken2);
            await QueryExecutor.Insert(mySqlConnection, queryInsertToken3);
            dbAccess.CloseConnection();
        }
    }
}
