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
    public class GetStore
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("logintest");
            StoreService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestGetStore()
        {
            ActionResult<string> result = await new StoreController().Get(1);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestGetNotExistingStore()
        {
            ActionResult<string> result = await new StoreController().Get(0);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}