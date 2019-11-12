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
    public class DeleteProduct
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

        }

        [Test]
        [Ignore("Not yet implemented")]
        public async Task TestWrongToken()
        {

        }

        [Test]
        [Ignore("Not yet implemented")]
        public async Task TestNoProductFound()
        {

        }
    }
}