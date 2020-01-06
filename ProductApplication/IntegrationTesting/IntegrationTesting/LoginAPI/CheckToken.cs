using LoginAPI.Controllers;
using LoginAPI.DataAccess;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using Shared.Data;
using Shared.Util;
using StockAPI.Controllers;
using StockAPI.LocalModels;
using System;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    public class CheckToken
    {
        [SetUp]
        public async Task Setup()
        {
            //await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            //string connectionString = DatabaseConnectionString.GetAzureConnectionString("logintest");
            //LoginService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrectToken()
        {
            ActionResult<string> result = await new LoginController().CheckToken("token1");
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestNotExistingToken()
        {
            ActionResult<string> result = await new LoginController().CheckToken("thisTokenDoesNotExist");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestTokenExpired()
        {

            ActionResult<string> result = await new LoginController().CheckToken("token3");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}