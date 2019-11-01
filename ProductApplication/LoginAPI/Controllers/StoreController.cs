using LoginAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPI.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        //GET api/stores/5
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get(long storeId)
        {
            StoreData storeData = await StoreService.GetStoreById(storeId);
            if (storeData != null)
            {
                return Ok(Json.Serialize<StoreData>(storeData));
            }
            else
            {
                return BadRequest("No store found with this id");
            }
        }
    }
}
