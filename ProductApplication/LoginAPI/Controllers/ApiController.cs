using LoginAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private string[] endpoints = new string[] { "Hello World!"};

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() 
        {
            DataAccessImplMySql data = new DataAccessImplMySql();
            //string s = data.Test();
            //StoreData store = data.GetStoreById(83);
            //UserData user = data.GetUserByToken("abcdefghij4321");
            //return endpoints;
            //return new string[] { user.userId + user.storeId + user.username + user.firstName + user.lastName + user.birthDate};

            return new string[] { "" + data.RefreshToken("token1234") };
        }
    }
}