using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OrderApplication.Services;
using StockAPI.DataAccess;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Testing.IntegrationTesting.LoginAPI;
using StockAPI.Controllers;
using StockAPI.LocalModels;
using Shared.Data;

namespace Testing.IntegrationTesting.StockAPI
{
    public class GetMutationsCustomDate
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseStockDummyFiller.EmptyAndFillDatabases();
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("stocktest");
            StockService.dataAccess = new StockDataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            string dateTime = Json.Serialize<DateTime>(DateTime.Now.AddDays(-10));
            ActionResult<string> result = await controller.GetMutations(1, 10, dateTime);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            StockMutation[] mutations = Json.Deserialize<StockMutation[]>(resultValue);

            StockMutation[] expectedMutations = new StockMutation[]
            {
                new StockMutation(1, 1, 1, -2, MutationReason.Sold, DateTime.Now.AddDays(-9)),
                new StockMutation(2, 1, 1, -2, MutationReason.Sold, DateTime.Now.AddDays(-8)),
                new StockMutation(3, 1, 1, -3, MutationReason.Broken, DateTime.Now.AddDays(-7)),
                new StockMutation(4, 1, 1, 10, MutationReason.Bought, DateTime.Now.AddDays(-6)),
                new StockMutation(5, 1, 1, -4, MutationReason.Sold, DateTime.Now.AddDays(-4))
            };

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual(expectedMutations, mutations);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            StockController controller = ControllerCreator.CreateStockControllerWrongToken();
            string dateTime = Json.Serialize<DateTime>(DateTime.Now);
            ActionResult<string> result = await controller.GetMutations(1, 10, dateTime);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestProductNotExisting()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            string dateTime = Json.Serialize<DateTime>(DateTime.Now);
            ActionResult<string> result = await controller.GetMutations(999999, 10, dateTime);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            StockMutation[] mutations = Json.Deserialize<StockMutation[]>(resultValue);

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.IsNull(mutations);
            });
        }
    }
}