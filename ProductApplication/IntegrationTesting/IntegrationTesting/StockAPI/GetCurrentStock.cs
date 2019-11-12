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

namespace Testing.IntegrationTesting.StockAPI
{
    public class GetCurrentStock
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseStockDummyFiller.EmptyAndFillDatabases();
            string connectionString = "server=localhost;port=3306;database=stocktest;user=root;password=root";
            StockService.dataAccess = new StockDataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            ActionResult<string> result = await controller.GetCurrentStock(1);
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                int? resultValue = (result.Result as OkObjectResult).Value as int?;
                Assert.NotNull(resultValue);
                Assert.AreEqual(9, resultValue);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            StockController controller = ControllerCreator.CreateStockControllerWrongToken();
            ActionResult<string> result = await controller.GetCurrentStock(1);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}