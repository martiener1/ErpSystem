using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services;
using Shared.Models;
using Shared.Util;

namespace ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrlLoginAPI = "https://localhost:5001/api/";

        public ProductController(IHttpClientFactory httpClientFactory)
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

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAll()
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product[] productArray = await ProductService.GetAllProducts((int)user.storeId);
            string[] returnArray = new string[productArray.Length];
            for (int i = 0; i < productArray.Length; i++)
            {
                returnArray[i] = Json.Serialize<Product>(productArray[i]);
            }
            return Ok(returnArray);
        }

        // GET api/products/223
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetById(long id)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product product = await ProductService.GetProductById((int)user.storeId, id);
            if (product == null)
            {
                return BadRequest("No product found with this Id");
            }
            return Ok(Json.Serialize<Product>(product));
        }

        // GET api/products/ean/234876345637
        [HttpGet("ean/{ean}")]
        public async Task<ActionResult<string>> GetProductByEan(string ean)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product product = await ProductService.GetProductByEAN((int)user.storeId, ean);
            if (product == null)
            {
                return BadRequest("No product found with this Id");
            }
            return Ok(Json.Serialize<Product>(product));
        }

        // GET api/products/productnumber/234876
        [HttpGet("productnumber/{productNumber}")]
        public async Task<ActionResult<string>> GetByProductNumber(string productNumber)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product product = await ProductService.GetProductByProductNumber((int)user.storeId, productNumber);
            if (product == null)
            {
                return BadRequest("No product found with this Id");
            }
            return Ok(Json.Serialize<Product>(product));
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] string value)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product product = await ProductService.AddNewProduct((int)user.storeId, Json.Deserialize<Product>(value));
            return Ok(Json.Serialize<Product>(product));
        }

        // PUT api/products/223
        [HttpPut("{value}")]
        public async Task<ActionResult<string>> Put(long id, [FromBody] string value)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            Product productNew = Json.Deserialize<Product>(value);
            productNew.id = id;
            Product product = await ProductService.AlterProduct((int)user.storeId, productNew);
            return Ok(Json.Serialize<Product>(product));
        }
        
        // DELETE api/products/223
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            UserData user = await GetUserData(Request.Headers["token"]);
            if (user == null) return BadRequest("No valid session found for this token");
            if (user.storeId == null) return BadRequest("No store found for this user");
            await ProductService.DeleteProduct((int)user.storeId, id);
            return Ok("Product deleted succesfully");
        }
    }
}
