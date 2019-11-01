﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shared.Data;
using Shared.Models;

namespace OrderApplication.DataAccess
{
    public class OrderDataAccessImplMySql : OrderDataAccess
    {
        string dbConnectionString;

        public OrderDataAccessImplMySql(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public OrderDataAccessImplMySql()
        {
            this.dbConnectionString = "server=localhost;port=3306;database=order;user=root;password=root";
        }

        private MySqlConnection NewConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            dbCon.Connect(dbConnectionString);
            return dbCon.Connection;
        }

        private void CloseConnection()
        {
            DBConnection.Instance().Close();
        }
        public async Task ChangeNextOrder(int storeId, long productId, int amount)
        {
            string query = "update nextorder set amount = @0 where storeId = @1 and productId = @2;";
            int rowsUpdated = await QueryExecutor.Update(NewConnection(), query, MySqlDbType.Int32, amount, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId);
            if (rowsUpdated == 0)
            {
                await CreateNewOrder(storeId, productId, amount);
            }
        }

        public async Task CreateNewOrder(int storeId, long productId, int amount)
        {
            string query = "INSERT INTO `order`.`nextorder` (`storeId`, `productId`, `amount`) VALUES (@0, @1, @2)";
            await QueryExecutor.Insert(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId, MySqlDbType.Int32, amount);
        }

        public async Task<int?> GetNextOrderAmount(int storeId, long productId)
        {
            string query = "select amount from nextorder where storeId = @0 and productId = @1;";
            object[] result = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, storeId, MySqlDbType.Int32, productId);

            if (result[0] == null) return null; // empty record
            else return (int)result[0];
        }
    }
}