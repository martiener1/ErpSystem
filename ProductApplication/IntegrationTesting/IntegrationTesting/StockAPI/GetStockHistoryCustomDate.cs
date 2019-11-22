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
using Shared.Data;

namespace Testing.IntegrationTesting.StockAPI
{
    public class GetStockHistoryCustomDate
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
            string dateTimeString = Json.Serialize<DateTime>(DateTime.Now.AddDays(-10));
            ActionResult<string> result = await controller.GetStockHistory(1, 3, dateTimeString);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            int[] stockHistory = Json.Deserialize<int[]>(resultValue);
            int[] expectedStockHistory = new int[] { 10, 10, 8};

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
            string dateTimeString = Json.Serialize<DateTime>(DateTime.Now.AddDays(-10));
            ActionResult<string> result = await controller.GetStockHistory(1, 3, dateTimeString);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task TestProductNotExisting()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            string dateTimeString = Json.Serialize<DateTime>(DateTime.Now.AddDays(-10));
            ActionResult<string> result = await controller.GetStockHistory(999999, 3, dateTimeString);
            string resultValue = (result.Result as OkObjectResult).Value as string;
            int[] stockHistory = Json.Deserialize<int[]>(resultValue);
            int[] expectedStockHistory = new int[] { 0, 0, 0};

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual(expectedStockHistory, stockHistory);
            });
        }
    }
}