using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;
using StockAPI.LocalModels;

namespace OrderApplication.DataAccess
{
    interface StockDataAccess
    {

        Task AddMutation(int storeId, StockMutation stockMutation);

        Task AddMutationBulk(int storeId, StockMutation[] stockMutations);

        Task<int?> GetCurrentStock(int storeId, long productId);

        Task<int[]> GetStockHistory(int storeId, long productId, int timeSpanInDays, DateTime startingDate);

        Task<StockMutation[]> GetMutationHistory(int storeId, long productId, int timeSpanInDays, DateTime startingDate);
    }
}
