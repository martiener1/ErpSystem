using LoginAPI.DataAccess;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPI.Services
{
    public class StoreService
    {
        public static DataAccess.DataAccess dataAccess = new DataAccessImplMySql();

        public static async Task<StoreData> GetStoreById(long id)
        {
            return await dataAccess.GetStoreById(id);
        }
    }
}
