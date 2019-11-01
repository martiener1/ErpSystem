using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrlLoginAPI = "https://localhost:5001/api/";

        public ApiController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            return Ok(user.storeId);
        }

        private async Task<UserData> GetUserData(string token)
        {
            string apiUrl = baseUrlLoginAPI + "login/checktoken/" + token;
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(apiUrl);
            string s = await response.Content.ReadAsStringAsync();
            UserData user = Json.Deserialize<UserData>(s);
            return user;
        }

        /*
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            HttpResponseMessage result = await client.GetAsync("/api/login/checktoken/token1234");
            result.StatusCode.ToString();
            result.Content.ToString();
            return Ok(result.Content.ToString());
        }*/
    }
}