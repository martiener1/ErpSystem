using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApplication.DataAccess;
using Shared.Models;

namespace OrderApplication.Services
{
    public static class OrderService
    {

        private static OrderDataAccess dataAccess = new OrderDataAccessImplMySql();
        
        public static async Task<int?> GetNextOrderAmount(int storeId, long productId)
        {
            return await dataAccess.GetNextOrderAmount(storeId, productId);
        }

        public static async Task UpdateNextOrderAmount(int storeId, long productId, int amount)
        {
                await dataAccess.ChangeNextOrder(storeId, productId, amount);
        }

        private static async Task CreateNextOrder(int storeId, long productId, int amount)
        {
            await dataAccess.CreateNewOrder(storeId, productId, amount);
        }
    }
}
