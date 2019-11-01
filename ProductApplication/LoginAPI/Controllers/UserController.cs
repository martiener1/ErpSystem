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
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //GET api/users/5
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get(long userId)
        {
            // get user from service
            // convert user to json
            UserData userData = await UserService.GetUserById(userId);
            if (userData != null)
            {
                return Ok(Json.Serialize<UserData>(userData));
            }
            else
            {
                return BadRequest("No user found with this id");
            }
        }
    }
}
