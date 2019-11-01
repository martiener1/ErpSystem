using LoginAPI.Controllers;
using LoginAPI.DataAccess;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    public class GetUser
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            string connectionString = "server=localhost;port=3306;database=logintest;user=root;password=root";
            UserService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestGetStore()
        {
            ActionResult<string> result = await new UserController().Get(1);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestGetNotExistingStore()
        {
            ActionResult<string> result = await new UserController().Get(0);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}