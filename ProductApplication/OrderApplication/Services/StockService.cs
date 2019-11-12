using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAPI.DataAccess;
using Shared.Models;
using StockAPI.LocalModels;
using Shared.Util;

namespace OrderApplication.Services
{
    public static class StockService
    {

        public static StockDataAccess dataAccess = new StockDataAccessImplMySql();

        public static async Task AddMutation(int storeId, StockMutation mutation)
        {
            await dataAccess.AddMutation(storeId, mutation);
        }

        public static async Task AddMutationBulk(int storeId, StockMutation[] mutations)
        {
            await dataAccess.AddMutationBulk(storeId, mutations);
        }

        public static async Task<int?> GetCurrentStock(int storeId, long productId)
        {
            return await dataAccess.GetCurrentStock(storeId, productId);
        }

        public static async Task<int[]> GetRecentStockHistory(int storeId, long productId, int timeSpanInDays)
        {
            return await GetStockHistory(storeId, productId, timeSpanInDays, DateTime.Now.AddDays(-timeSpanInDays + 1));
        }

        public static async Task<int[]> GetStockHistory(int storeId, long productId, int timeSpanInDays, DateTime startingDate)
        {
            return await dataAccess.GetStockHistory(storeId, productId, timeSpanInDays, startingDate);
        }

        public static async Task<int[]> GetStockHistory(int storeId, long productId, int timeSpanInDays, string startingDateAsString)
        {
            return await dataAccess.GetStockHistory(storeId, productId, timeSpanInDays, ConvertStringToDateTime(startingDateAsString));
        }

        public static async Task<StockMutation[]> GetRecentStockMutations(int storeId, long productId, int timeSpanInDays)
        {
            return await GetStockMutations(storeId, productId, timeSpanInDays, DateTime.Now.AddDays(-timeSpanInDays));
        }

        public static async Task<StockMutation[]> GetStockMutations(int storeId, long productId, int timeSpanInDays, DateTime startingDate)
        {
            return await dataAccess.GetMutationHistory(storeId, productId, timeSpanInDays, startingDate);
        }

        public static async Task<StockMutation[]> GetStockMutations(int storeId, long productId, int timeSpanInDays, string startingDateAsString)
        {
            return await dataAccess.GetMutationHistory(storeId, productId, timeSpanInDays, ConvertStringToDateTime(startingDateAsString));
        }

        private static DateTime ConvertStringToDateTime(string dateTimeAsString)
        {
            return Json.Deserialize<DateTime>(dateTimeAsString);
        }
        
    }
}
