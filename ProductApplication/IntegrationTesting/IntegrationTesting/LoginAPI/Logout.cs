using LoginAPI.Controllers;
using LoginAPI.DataAccess;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shared.Data;
using System;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    public class Logout
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("logintest");
            LoginService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestRegularLogout()
        {
            ActionResult<string> result = await new LoginController().Logout("token1");
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestNotExistingToken()
        {
            ActionResult<string> result = await new LoginController().Logout("thisTokenDoesNotExist");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestExpiredToken()
        {
            ActionResult<string> result = await new LoginController().Logout("token3");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestTokenAlreadyLoggedOut()
        {
            await new LoginController().Logout("token1");
            ActionResult<string> result = await new LoginController().Logout("token1");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}