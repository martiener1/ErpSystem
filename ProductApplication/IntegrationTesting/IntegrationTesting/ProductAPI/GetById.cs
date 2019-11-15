using LoginAPI.Controllers;
using LoginAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ProductAPI.Controllers;
using ProductAPI.Services;
using ProductAPI.DataAccess;
using Shared.Models;
using Shared.Util;
using StockAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Testing.IntegrationTesting.LoginAPI;
using Shared.Data;

namespace Testing.IntegrationTesting.ProductAPI
{
    public class GetById
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            await DatabaseProductDummyFiller.EmptyAndFillDatabases();
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("producttest");
            ProductService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<string> result = await controller.GetById(1);
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Product productExpected = new Product(1, 1, "name1", "brand1", 1d, 2.5d, "supplier1", "123456", "8718123456721", new ProductGroup(1, "group1", new ProductCategory(1, "category1")));
                string resultValue = (result.Result as OkObjectResult).Value as string;
                Product productActual = Json.Deserialize<Product>(resultValue);
                Assert.AreEqual(productExpected, productActual);
            });
        }

        [Test]
        public async Task TestWrongToken()
        {
            ProductController controller = ControllerCreator.CreateProductControllerWrongToken();
            ActionResult<string> result = await controller.GetById(1);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestWrongId()
        {
            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<string> result = await controller.GetById(3125);
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}