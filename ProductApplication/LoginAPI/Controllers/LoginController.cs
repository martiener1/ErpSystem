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
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET api/login/login/{username}/{password}
        [HttpGet("login/{username}/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            // THIS SHOULD NOT BE DONE THIS WAY
            // THIS IS NOT SECURE FOR THE USER
            string token = await LoginService.Login(username, password);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("Wrong Credentials");
            }
        }

        // GET api/login/checktoken/{token}
        [HttpGet("checktoken/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CheckToken(string token)
        {
            UserData userData = await LoginService.CheckTokenValidity(token);
            if (userData != null)
            {
                return Ok(Json.Serialize<UserData>(userData));
            }
            else
            {
                return BadRequest("No session found with this token");
            }
        }

        // POST api/login/logout
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Logout([FromBody] string token)
        {
            if (await LoginService.Logout(token))
            {
                return Ok("User Logged Out");
            }
            else
            {
                return BadRequest("No session found with this token");
            }
        }
    }
}
