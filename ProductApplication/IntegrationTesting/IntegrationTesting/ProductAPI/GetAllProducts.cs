using LoginAPI.Controllers;
using LoginAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ProductAPI.Controllers;
using ProductAPI.DataAccess;
using ProductAPI.Services;
using Shared.Data;
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

namespace Testing.IntegrationTesting.ProductAPI
{
    public class GetAllProducts
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
        [Ignore("GetAll() Is not yet implemented")]
        public async Task TestCorrect()
        {
            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<IEnumerable<string>> result = await controller.GetAll();
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        [Ignore("GetAll() Is not yet implemented")]
        public async Task TestWrongToken()
        {
            ProductController controller = ControllerCreator.CreateProductControllerWrongToken();
            ActionResult<IEnumerable<string>> result = await controller.GetAll();
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}