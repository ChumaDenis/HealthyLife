using HealthyLife.Models;
using HealthyLife.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HealthyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUser _authContext;

        public AuthController(IConfiguration configuration, IUser authContext)
        {
            _configuration = configuration;
            _authContext = authContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserDTO request)
        {
            if (!_authContext.CheckUser(request))
            {
                _authContext.AddUser(request);
                return Ok("User add");
            }
            
            return BadRequest("This user is already registered!");
           
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            if (!_authContext.CheckUser(request))
            {
                return BadRequest("User not found.");
            }
            string token = _authContext.CreateToken(request);
            return Ok(token);
        }

        [HttpGet("check"), Authorize]
        public async Task<ActionResult<string>> Ch()
        {
            
            return Ok("Ok");
        }



        [HttpPost("change")]
        public async Task<ActionResult<string>> Change(UserDTO request)
        {
            if (!_authContext.CheckUser(request))
            {
                return BadRequest("User not found.");
            }
            string token = _authContext.CreateToken(request);
            return Ok(token);
        }



    }
}
