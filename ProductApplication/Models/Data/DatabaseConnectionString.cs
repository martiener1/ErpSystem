using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Data
{
    public class DatabaseConnectionString
    {

        static readonly string baseString = "server={0};port={1};database={2};user={3};password={4}";

        static readonly string azureString = "server = erpsystemdatabase.mysql.database.azure.com;port=3306;database={0};user=martijn@erpsystemdatabase;password=Password!";

        public static string Get(string server, string port, string database, string user, string password)
        {
            return string.Format(baseString, server, port, database, user, password);
        }

        public static string GetAzureConnectionString(string database)
        {
            return string.Format(azureString, database);
        }

    }
}
