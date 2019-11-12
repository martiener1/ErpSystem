using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testing.IntegrationTesting
{
    class DatabaseAccess
    {
        private string dbConnectionString;

        public DatabaseAccess(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public MySqlConnection NewConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            dbCon.Connect(dbConnectionString);
            return dbCon.Connection;
        }

        public MySqlConnection GetConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            MySqlConnection connection = dbCon.Connection;
            if (connection != null)
            {
                return connection;
            } else
            {
                return NewConnection();
            }
        }

        public void CloseConnection()
        {
            DBConnection.Instance().Close();
        }
    }
}
