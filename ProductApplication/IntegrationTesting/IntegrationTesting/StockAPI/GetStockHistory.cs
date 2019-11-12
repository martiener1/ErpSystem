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
    public class GetStockHistory
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
            ActionResult<string> result = await controller.GetStockHistory(1, 10);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            int[] stockHistory = Json.Deserialize<int[]>(resultValue);
            int[] expectedStockHistory = new int[] {10, 8, 6, 3, 13, 13, 9, 9, 9, 9 };

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual(expectedStockHistory, stockHistory);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            StockController controller = ControllerCreator.CreateStockControllerWrongToken();
            ActionResult<string> result = await controller.GetStockHistory(1, 10);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestProductNotExisting()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            ActionResult<string> result = await controller.GetStockHistory(999999, 10);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            int[] stockHistory = Json.Deserialize<int[]>(resultValue);
            int[] expectedStockHistory = new int[] { 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 };

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual(expectedStockHistory, stockHistory);
            });
        }
    }
}