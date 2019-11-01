using LoginAPI.DataAccess;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAPI.Services
{
    public class LoginService
    {
        public static DataAccess.DataAccess dataAccess = new DataAccessImplMySql();

        public static async Task<string> Login(string username, string password)
        {
            UserData userData = await dataAccess.GetUserByCredentials(username, password);
            if (userData != null) {
                string token = CreateRandomToken(userData);
                if (await dataAccess.ReplaceToken(userData.userId, token))
                {
                    return token;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static async Task<UserData> CheckTokenValidity(string token)
        {
            if (await dataAccess.IsTokenValid(token))
            {
                await dataAccess.RefreshToken(token);
                return await dataAccess.GetUserByToken(token);
            }
            else
            {
                return null;
            }
        }

        public static async Task<bool> Logout(string token)
        {
            if (await dataAccess.IsTokenValid(token))
            {
                await dataAccess.SetTokenExpired(token);
                return true;
            }
            else return false;
        }

        private static string CreateRandomToken(UserData userData)
        {
            string firstString = new StringBuilder().Append(userData.firstName).Append(userData.storeId).Append(DateTime.Now.Millisecond).ToString();
            //TODO: change the token by shifting ASCII codes
            return firstString;

        }
    }
}
