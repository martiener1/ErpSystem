using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Shared.Models;

namespace LoginAPI.DataAccess
{
    public class DataAccessImplMySql : DataAccess
    {
        string dbConnectionString;

        public DataAccessImplMySql(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public DataAccessImplMySql()
        {
            this.dbConnectionString = DatabaseConnectionString.GetAzureConnectionString("login");
        }

        private MySqlConnection NewConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            dbCon.Connect(dbConnectionString);
            return dbCon.Connection;
        }

        private void CloseConnection()
        {
            //DBConnection.Instance().Close();
        }

        public async Task<bool> AreCredentialsOk(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreData> GetStoreById(long storeId)
        {
            string query = "select * from stores where id = @0;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, storeId);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            string name = (string)row[1];
            string city = (string)row[2];
            string address = (row[3] == DBNull.Value) ? null : (string)row[3]; // address is nullable
            return new StoreData(id, name, city, address);
        }

        public async Task<UserData> GetUserByCredentials(string username, string password)
        {
            string query = "select u.id, s.id, u.loginname, u.firstname, u.lastname, u.birthdate from users u, stores s where u.storeid = s.id and u.loginname = @0 and u.password = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.VarChar, username, MySqlDbType.VarChar, password);
            CloseConnection();

            if (row[0] == null) return null; // empty record
            int uId = (int)row[0];
            int? storeId = (row[1] == null) ? null : (int?)row[1];
            string loginname = (string)row[2];
            string firstName = (row[3] == DBNull.Value) ? null : (string)row[3];
            string lastName = (row[4] == DBNull.Value) ? null : (string)row[4];
            DateTime? birthDate = (row[5] == DBNull.Value) ? null : (DateTime?)row[5];
            return new UserData(uId, storeId, loginname, firstName, lastName, birthDate);
        }

        public async Task<UserData> GetUserById(long userId)
        {
            string query = "select u.id, s.id, u.loginname, u.firstname, u.lastname, u.birthdate from users u, stores s where u.storeid = s.id and u.id = @0;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, userId);
            CloseConnection();

            if (row[0] == null) return null; // empty record
            int uId = (int)row[0];
            int? storeId = (row[1] == null) ? null : (int?)row[1];
            string username = (string)row[2];
            string firstName = (row[3] == DBNull.Value) ? null : (string) row[3];
            string lastName = (row[4] == DBNull.Value) ? null : (string)row[4];
            DateTime? birthDate = (row[5] == DBNull.Value) ? null : (DateTime?)row[5];
            return new UserData(uId, storeId, username, firstName, lastName, birthDate);
        }

        public async Task<UserData> GetUserByToken(string token)
        {
            string query = "select u.id, s.id, u.loginname, u.firstname, u.lastname, u.birthdate from users u, stores s, tokens t where t.userid = u.id and u.storeid = s.id and t.token = @0;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.VarChar, token);
            CloseConnection();

            if (row[0] == null) return null; // empty record
            int uId = (int)row[0];
            int? storeId = (row[1] == null) ? null : (int?)row[1];
            string username = (string)row[2];
            string firstName = (row[3] == DBNull.Value) ? null : (string)row[3];
            string lastName = (row[4] == DBNull.Value) ? null : (string)row[4];
            DateTime? birthDate = (row[5] == DBNull.Value) ? null : (DateTime?)row[5];
            return new UserData(uId, storeId, username, firstName, lastName, birthDate);
        }

        public async Task<bool> IsTokenValid(string token)
        {
            string query = "select expirationdate > now() from tokens where token = @0;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.VarChar, token);
            CloseConnection();

            if (row[0] == null)
            {
                return false;
            } else
            {
                int isValid = (int)(long)row[0];
                if (isValid == 1)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public async Task<bool> RefreshToken(string token)
        {
            string query = "UPDATE tokens SET expirationdate = ADDTIME(now(), '00:30:00') WHERE token = @0 and expirationdate > now();";
            int rowsAffected = await QueryExecutor.Update(NewConnection(), query, MySqlDbType.VarChar, token);
            CloseConnection();
            return rowsAffected > 0;
        }

        public async Task<bool> ReplaceToken(long userId, string newToken)
        {
            await DeleteTokens(userId);
            return await NewToken(userId, newToken);
        }

        private async Task DeleteTokens(long userId)
        {
            string query = "delete from tokens where userid = @0;";
            await QueryExecutor.Delete(NewConnection(), query, MySqlDbType.Int32, userId);
            CloseConnection();
        }

        private async Task<bool> NewToken(long userId, string newToken)
        {
            string query = "insert into tokens (userid, token, expirationdate) values (@0, @1, ADDTIME(now(), '00:30:00'));";
            int rowsAffected = await QueryExecutor.Insert(NewConnection(), query, MySqlDbType.Int32, userId, MySqlDbType.VarChar, newToken);
            CloseConnection();
            return rowsAffected > 0;
        }

        public async Task SetTokenExpired(string token)
        {
            string query = "UPDATE tokens SET expirationdate = now() WHERE token = @0 and expirationdate > now();";
            await QueryExecutor.Update(NewConnection(), query, MySqlDbType.VarChar, token);
            CloseConnection();
        }
    }
}
