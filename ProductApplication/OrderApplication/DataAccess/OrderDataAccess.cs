using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace OrderApplication.DataAccess
{
    interface OrderDataAccess
    {

        Task<int?> GetNextOrderAmount(int storeId, long productId);

        Task ChangeNextOrder(int storeId, long productId, int amount);

        Task CreateNewOrder(int storeId, long productId, int amount);
    }
}
