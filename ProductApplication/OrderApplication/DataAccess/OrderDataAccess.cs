using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace StockAPI.DataAccess
{
    public interface OrderDataAccess
    {

        Task<int?> GetNextOrderAmount(int storeId, long productId);

        Task<bool> ChangeNextOrder(int storeId, long productId, int amount);

        Task CreateNewOrder(int storeId, long productId, int amount);
    }
}
