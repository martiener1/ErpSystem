using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using ProductAPI.Controllers;
using Shared.Models;
using Shared.Util;
using StockAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Testing.IntegrationTesting
{
    class ControllerCreator
    {

        public static ProductController CreateProductControllerCorrectToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryOk();
            ProductController controller = new ProductController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        public static ProductController CreateProductControllerWrongToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryBadRequest();
            ProductController controller = new ProductController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        public static OrderController CreateOrderControllerCorrectToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryOk();
            OrderController controller = new OrderController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        public static OrderController CreateOrderControllerWrongToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryBadRequest();
            OrderController controller = new OrderController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        public static StockController CreateStockControllerCorrectToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryOk();
            StockController controller = new StockController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        public static StockController CreateStockControllerWrongToken()
        {
            var httpClientFactoryMock = CreateMockClientFactoryBadRequest();
            StockController controller = new StockController(httpClientFactoryMock);
            AddToken(controller, "token", "token1");
            return controller;
        }

        private static void AddToken(ControllerBase controller, string key, string value)
        {
            var httpRequestMock = Substitute.For<HttpRequest>();
            IHeaderDictionary headers = new HeaderDictionary();
            headers.Add(key, value);
            httpRequestMock.Headers.Returns(headers);

            var httpContextMock = Substitute.For<HttpContext>();
            httpContextMock.Request.Returns(httpRequestMock);
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContextMock;
            controller.ControllerContext = controllerContext;
        }

        private static IHttpClientFactory CreateMockClientFactoryOk()
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(GetOkHttpResponse());
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);
            return httpClientFactoryMock;
        }

        private static IHttpClientFactory CreateMockClientFactoryBadRequest()
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(GetBadRequestHttpResponse());
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);
            return httpClientFactoryMock;
        }

        private static HttpResponseMessage GetOkHttpResponse()
        {
            UserData userData = new UserData(1, 1, "TestUsername", "TestFirst", "TestLast", DateTime.Now);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Json.Serialize<UserData>(userData), Encoding.UTF8, "application/json")
            };
        }

        private static HttpResponseMessage GetBadRequestHttpResponse()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };
        }
    }
}
