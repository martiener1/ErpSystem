using LoginAPI.Controllers;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using OrderApplication.Services;
using StockAPI.DataAccess;
using StockAPI.Controllers;
using Shared.Data;

namespace Testing.IntegrationTesting.OrderAPI
{
    public class PutNextOrder
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseOrderDummyFiller.EmptyAndFillDatabases();
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("ordertest");
            OrderService.dataAccess = new OrderDataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerCorrectToken();
            ActionResult<string> result = await controller.Put(1, 20);

            controller = ControllerCreator.CreateOrderControllerCorrectToken();
            ActionResult<string> resultAfter = await controller.Get(1);
            int? resultValue = (resultAfter.Result as OkObjectResult).Value as int?;
            
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.AreEqual(20, resultValue);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerWrongToken();
            ActionResult<string> result = await controller.Put(1, 20);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task TestNoOrderFound()
        {
            OrderController controller = ControllerCreator.CreateOrderControllerCorrectToken();
            ActionResult<string> result = await controller.Put(10000, 20);
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }
    }
}