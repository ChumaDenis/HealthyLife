using HealthyLife.Models.Auth;
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
        public async Task<ActionResult<string>> Register(UserRegisterRequest request)
        {
            if (!_authContext.CheckUser(request.Email))
            {
                _authContext.AddUser(request);
                return Ok("User add");
            }
            
            return BadRequest("This user is already registered!");
           
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginRequest request)
        {
            try
            {
                if (!_authContext.CheckUser(request.Email))
                {
                    throw new Exception ("User not found.");
                }
                string token = _authContext.CreateToken(request);
                return Ok(token);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPost("verify")]
        public async Task<ActionResult<string>> VerifyUser(string token)
        {
            try
            {
                _authContext.VerifyUser(token);
                return Ok("The user has been successfully verified!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("check"), Authorize]
        public async Task<ActionResult<string>> Ch()
        {
            
            return Ok("Ok");
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<string>> ForgotPassword(string email)
        {
            try
            {
                string token = _authContext.PasswordResetAcces(email);
                return Ok(token);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<string>> ChangePassword(ResetPasswordRequest request)
        {
            try
            {
                _authContext.PasswordReset(request);
                return Ok("The password has been changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }



    }
}
