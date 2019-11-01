using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPI.DataAccess
{
    public interface DataAccess
    {
        Task<UserData> GetUserById(long userId);

        Task<StoreData> GetStoreById(long storeId);

        Task<bool> AreCredentialsOk(string username, string password);

        Task<UserData> GetUserByCredentials(string username, string password);

        Task<bool> ReplaceToken(long userId, string newToken);

        Task<bool> IsTokenValid(string token);

        Task<UserData> GetUserByToken(string token);

        Task<bool> RefreshToken(string token);

        Task SetTokenExpired(string token);

    }
}
