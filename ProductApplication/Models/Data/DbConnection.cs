using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace Shared.Data
{
    public class DBConnection
    {
        private DBConnection()
        {
        }
        
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnected()
        {
            if (Connection == null)
            {
                return false;
            }

            return true;
        }

        public bool Connect(string connString)
        {
            if (IsConnected())
            {
                connection.Dispose();
            }
            connection = new MySqlConnection(connString);
            //connection.Open();
            return true;

        }

        public void Close()
        {
            connection.Close();
            connection.Dispose();
            connection = null;
        }
    }
}