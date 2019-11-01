﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using OrderApplication.Services;
using StockAPI.LocalModels;
using System.Net.Http;
using Shared.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockAPI.Controllers
{
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrlLoginAPI = "https://localhost:5001/api/";

        public StockController(IHttpClientFactory httpClientFactory)
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

        // GET api/stock/current/123456                    return the current stuck of product with id 123456
        [HttpGet("current/{id}")]
        public async Task<ActionResult<string>> GetCurrentStock(long id)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            int? currentStock = await StockService.GetCurrentStock((int)user.storeId, id);
            if (currentStock == null)
            {
                return NotFound("The current stock could not be found or calculated");
            }
            return Ok(currentStock);
        }

        // POST api/stock/mutations                             post new stockmutation
        [HttpPost("mutations")]
        public async Task<ActionResult<string>> PostMutation([FromBody] string value)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            await StockService.AddMutation((int)user.storeId, Shared.Util.Json.Deserialize<StockMutation>(value));
            return Ok("Mutation has been posted");
        }

        // POST api/stock/mutations/bulk                        post new stockmutations in bulk (store multiple mutations in one call)
        [HttpPost("mutations/bulk")]
        public async Task<ActionResult<string>> PostBulkMutation([FromBody] string value)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            await StockService.AddMutationBulk((int)user.storeId, Shared.Util.Json.Deserialize<StockMutation[]>(value));
            return Ok("Mutations have been posted");
        }

        // GET api/stock/history/123456/10                  return the stock from the most recent 10 days, one value for every end of the day
        [HttpGet("history/{id}/{days}")]
        public async Task<ActionResult<string>> GetStockHistory(long id, int days)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            int[] stockHistory = await StockService.GetRecentStockHistory((int)user.storeId, id, days);
            return Ok(Shared.Util.Json.Serialize<int[]>(stockHistory));
        }

        // GET api/stock/history/123456/10/20052018         return the stock at period 20-05-2018 to 29-05-2018, values at the end of the day
        [HttpGet("history/{id}/{days}/{date}")]
        public async Task<ActionResult<string>> GetStockHistory(long id, int days, string date)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            int[] stockHistory = await StockService.GetStockHistory((int)user.storeId, id, days, date);
            return Ok(Shared.Util.Json.Serialize<int[]>(stockHistory));
        }

        // GET api/stock/mutations/123456/10/20052018       return the stockmutations at period 20-05-2018 to 29-05-2018, values at the end of the day
        [HttpGet("mutations/{id}/{days}/{date}")]
        public async Task<ActionResult<string>> GetMutations(long id, int days, string date)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            StockMutation[] mutationHistory = await StockService.GetStockMutations((int)user.storeId, id, days, date);
            return Ok(Shared.Util.Json.Serialize<StockMutation[]>(mutationHistory));
        }

        // GET api/stock/mutations/123456/10                return the stockmutations from the most recent 10 days
        [HttpGet("mutations/{id}/{days}")]
        public async Task<ActionResult<string>> GetMutations(long id, int days)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            StockMutation[] mutationHistory = await StockService.GetRecentStockMutations((int)user.storeId, id, days);
            return Ok(Shared.Util.Json.Serialize<StockMutation[]>(mutationHistory));
        }
    }
}