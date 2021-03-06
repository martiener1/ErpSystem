﻿using Microsoft.AspNetCore.Mvc;
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
using StockAPI.Services;

namespace Testing.IntegrationTesting.StockAPI
{
    public class PostSingleMutation
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
            StockMutation mutation = new StockMutation(null, 1, 1, -1, MutationReason.Sold, DateTime.Now.AddDays(-1));
            string mutationString = Json.Serialize<StockMutation>(mutation);
            ActionResult<string> result = await controller.PostMutation(mutationString);
            Assert.Multiple(async () =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);

                ActionResult<string> resultCheck = await controller.GetCurrentStock(1);
                int? resultValue = (resultCheck.Result as OkObjectResult).Value as int?;
                Assert.NotNull(resultValue);
                Assert.AreEqual(8, resultValue);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            StockController controller = ControllerCreator.CreateStockControllerWrongToken();
            StockMutation mutation = new StockMutation(null, 1, 1, -1, MutationReason.Sold, DateTime.Now.AddDays(-1));
            string mutationString = Json.Serialize<StockMutation>(mutation);
            ActionResult<string> result = await controller.PostMutation(mutationString);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task TestWrongString()
        {
            StockController controller = ControllerCreator.CreateStockControllerCorrectToken();
            ActionResult<string> result = await controller.PostMutation("This string is not a StockMutation");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}