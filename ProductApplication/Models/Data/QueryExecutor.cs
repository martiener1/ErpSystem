using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data
{
    public class QueryExecutor
    {
        private static MySqlCommand CreateMySqlCommand(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            int paramsCount = dataTypesAndValues.Length;
            if (paramsCount % 2 != 0) return null; // dataTypesAndValues should be even, every parameter should have a MySqlDbType
            for (int i = 0; i < paramsCount / 2; i++)
            {
                command.Parameters.Add(string.Format("@{0}", i), (MySqlDbType)dataTypesAndValues[2 * i]).Value = dataTypesAndValues[2 * i + 1];
            }
            return command;
        }

        public static async Task<object[]> SelectSingle(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            // usage : SelectSingle(connection, "SELECT * FROM table WHERE city = @0 AND name = @1", MySqlDbType.VarChar, "cityName", MySqlDbType.VarChar, "name");
            await connection.OpenAsync();
            MySqlCommand command = CreateMySqlCommand(connection, query, dataTypesAndValues);
            DbDataReader reader = await command.ExecuteReaderAsync();
            
            object[] currentRow = new object[reader.FieldCount];
            while (reader.Read())
            {
                reader.GetValues(currentRow);
            }
            reader.Close();
            await connection.CloseAsync();
            return currentRow;
        }

        public static async Task<object[][]> SelectMultiple(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            // usage : SelectMultiple(connection, "SELECT * FROM table WHERE city = @0 AND name = @1", MySqlDbType.VarChar, "cityName", MySqlDbType.VarChar, "name");
            MySqlCommand command = CreateMySqlCommand(connection, query, dataTypesAndValues);
            DbDataReader reader = await command.ExecuteReaderAsync();

            object[][] returnRows = new object[0][];
            while (reader.Read())
            {
                object[] currentRow = new object[reader.FieldCount];
                reader.GetValues(currentRow);
                Array.Resize(ref returnRows, returnRows.Length + 1);
                returnRows[returnRows.GetUpperBound(0)] = currentRow;
            }
            reader.Close();
            return returnRows;
        }

        public static async Task<int> Update(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            return await ExecuteNonQuery(connection, query, dataTypesAndValues);
        }

        public static async Task<int> Delete(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            return await ExecuteNonQuery(connection, query, dataTypesAndValues);
        }

        public static async Task<int> Insert(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            return await ExecuteNonQuery(connection, query, dataTypesAndValues);
        }

        public static async Task Truncate(MySqlConnection connection, string query)
        {
            //TODO: Maybe find a way so only test databases can be truncated/wiped
            await ExecuteNonQuery(connection, query);
        }

        private static async Task<int> ExecuteNonQuery(MySqlConnection connection, string query, params object[] dataTypesAndValues)
        {
            MySqlCommand command = CreateMySqlCommand(connection, query, dataTypesAndValues);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
}
}
