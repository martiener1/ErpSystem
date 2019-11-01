using LoginAPI.Controllers;
using LoginAPI.DataAccess;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    public class Login
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            string connectionString = "server=localhost;port=3306;database=logintest;user=root;password=root";
            LoginService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrectCredentials()
        {
            ActionResult<string> result = await new LoginController().Login("user1", "password");
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestWrongCredentials()
        {
            ActionResult<string> result = await new LoginController().Login("user1", "thisIsNotTheCorrectPassword");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestAlreadyLoggedInUser()
        {
            await new LoginController().Login("user1", "password");
            ActionResult<string> result = await new LoginController().Login("user1", "password");
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }
    }
}