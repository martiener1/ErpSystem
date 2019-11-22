using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using OrderApplication.Services;
using Shared.Util;
using System.Net.Http;


namespace StockAPI.Controllers
{
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrlLoginAPI = "https://martijnloginapi.azurewebsites.net/api/";

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        private async Task<UserData> GetUserData(string token)
        {

            string apiUrl = baseUrlLoginAPI + "login/checktoken/" + token;
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(apiUrl);
            string s = await response.Content.ReadAsStringAsync();
            UserData user = Json.Deserialize<UserData>(s);
            return user;
        }

        // GET api/orders/nextorder/123456                       return the next scheduled order amount for product with id 123456
        // PUT api/orders/nextorder/123456/24                    change the next order amount for product 123456 to 24

        // GET api/orders/nextorder/123456
        [HttpGet("nextorder/{id}")]
        public async Task<ActionResult<string>> Get(int productId)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return Unauthorized("No valid session found for this token");
            if (user.storeId == null) return Unauthorized("No store found for this user");

            int? amount = await OrderService.GetNextOrderAmount((int)user.storeId, productId);
            if (amount == null) return NotFound("No order found");
            return Ok(amount);
        }

        // PUT api/orders/nextorder/123456/24
        [HttpPut("nextorder/{id}/{amount}")]
        public async Task<ActionResult<string>> Put(int productId, int amount)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return Unauthorized("No valid session found for this token");
            if (user.storeId == null) return Unauthorized("No store found for this user");

            bool updated = await OrderService.UpdateNextOrderAmount((int)user.storeId, productId, amount);
            if (updated)
            {
                return Ok("The next order has been updated");
            }
            else
            {
                return CreatedAtAction("Order didn't exist, new one is made", amount);
            }
        }
    }
}
