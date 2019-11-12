using Microsoft.AspNetCore.Mvc;
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
    public class PostProduct
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
        [Ignore("Not yet implemented")]
        public async Task TestCorrect()
        {
            Product newProduct = new Product(1, 1, "name1", "brand1", 1d, 2.5d, "supplier1", "123456", "8718123456721", new ProductGroup(1, "group1", new ProductCategory(1, "category1")));

            ProductController controller = ControllerCreator.CreateProductControllerCorrectToken();
            ActionResult<string> result = await controller.Post(Json.Serialize<Product>(newProduct));
            Product resultProduct = Json.Deserialize<Product>((result.Result as OkObjectResult).Value as string);

            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                Assert.Equals(newProduct, resultProduct);
            });
        }

        [Test]
        [Ignore("Not yet implemented")]
        public async Task TestWrongToken()
        {

        }

        [Test]
        [Ignore("Not yet implemented")]
        public async Task TestJsonNotReadible()
        {

        }
    }
}