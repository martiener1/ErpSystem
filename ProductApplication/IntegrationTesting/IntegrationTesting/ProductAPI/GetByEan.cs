﻿using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProductAPI.Controllers;
using ProductAPI.DataAccess;
using ProductAPI.Services;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Testing.IntegrationTesting.LoginAPI;

namespace Testing.IntegrationTesting.ProductAPI
{
    public class GetByEan
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseLoginTestFiller.EmptyAndFillDatabases();
            await DatabaseProductDummyFiller.EmptyAndFillDatabases();
            string connectionString = "server=localhost;port=3306;database=producttest;user=root;password=root";
            ProductService.dataAccess = new DataAccessImplMySql(connectionString);
        }

        [Test]
        public async Task TestCorrect()
        {
            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<string> result = await controller.GetProductByEan("8718123456721");
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
            ActionResult<string> result = await controller.GetProductByEan("8718123456721");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task TestWrongEan()
        {
            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<string> result = await controller.GetProductByEan("NotExisting");
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}