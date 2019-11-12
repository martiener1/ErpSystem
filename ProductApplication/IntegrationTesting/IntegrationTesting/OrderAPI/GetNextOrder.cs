﻿using LoginAPI.Controllers;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using OrderApplication.Services;
using StockAPI.DataAccess;
using StockAPI.Controllers;

namespace Testing.IntegrationTesting.OrderAPI
{
    public class GetNextOrder
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseOrderDummyFiller.EmptyAndFillDatabases();
            string connectionString = "server=localhost;port=3306;database=ordertest;user=root;password=root";
            OrderService.dataAccess = new OrderDataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerCorrectToken();
            ActionResult<string> result = await controller.Get(1);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task TestWrongToken()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerWrongToken();
            ActionResult<string> result = await controller.Get(1);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestNoOrderFound()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerCorrectToken();
            ActionResult<string> result = await controller.Get(10000);
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
    }
}