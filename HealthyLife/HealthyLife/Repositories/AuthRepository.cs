using HealthyLife.Contexts;
using HealthyLife.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HealthyLife.Repositories
{
    public class AuthRepository : IUser
    {
        readonly AuthContext _dbContext;

        private readonly IConfiguration _configuration;
        public AuthRepository(IConfiguration configuration, AuthContext dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }


        //
        public bool CheckUser(string Email)
        {
            return _dbContext.Users.Any(x=>x.Email==Email);
        }
        //
        public bool CheckUser(UserRegisterRequest request)
        {
             User? user = _dbContext.Users.FirstOrDefault(e => e.Email == request.Email);
             if (user != null)
             {
                return VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
             }
             return false;
        }
        //
        public bool CheckUser(UserLoginRequest request)
        {
            User? user = _dbContext.Users.FirstOrDefault(e => e.Email == request.Email);
            if (user != null)
            {
                return VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            }
            return false;
        }

        //
        public void AddUser(UserRegisterRequest request)
        {
            try
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = new User() {UserName=request.UserName, PasswordSalt = passwordSalt, 
                    PasswordHash= passwordHash, Email=request.Email, VerifacationToken= CreateRandomToken()};
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public User DeleteUser(string id)
        {
            try
            {
                User? user = _dbContext.Users.Find(id);

                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChanges();
                    return user;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public User GetUserDetails(string email)
        {
            User? user = _dbContext.Users.FirstOrDefault(x=>x.Email==email);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("User not found!");
            }
        }

        public string PasswordResetAcces(string email)
        {
            User user = GetUserDetails(email);
            user.PasswordRessetToken = CreateRandomToken();
            user.PasswordRessetExpires = DateTime.Now.AddDays(1);
            _dbContext.SaveChanges();

            return user.PasswordRessetToken;
        }



        public void PasswordReset(ResetPasswordRequest request)
        {
            User user = _dbContext.Users.FirstOrDefault(x => x.PasswordRessetToken == request.Token);
            if(user==null || user.PasswordRessetExpires < DateTime.Now)
            {
                throw new Exception("Invalid token!");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            user.PasswordRessetExpires = null;
            user.PasswordRessetToken = null;


            _dbContext.SaveChanges();
            
        }

        public void VerifyUser(string token)
        {
            User user = _dbContext.Users.FirstOrDefault(x => x.VerifacationToken == token);
            if (user == null)
            {
                throw new Exception("User don`t found!");
            }

            if (user.VerifiedAt != null)
            {
                throw new Exception("The user has already been verified!");
            }
            user.VerifiedAt = DateTime.Now;
            _dbContext.SaveChanges();
        }



        public bool CheckToken(string value)
        {
            return _dbContext.Tokens.Any(e => e.Value==value && e.CreateDate>=DateTime.Now);
        }

        public string CreateToken(UserLoginRequest request)
        {
            User? user = _dbContext.Users.First(e => e.Email == request.Email);
            if (VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                //Token newToken= new Token() { Value= jwt, UserId=user.Id};
                //_dbContext.Tokens.Add(newToken);
                //_dbContext.SaveChanges();

                return jwt;
            }
            return "error";
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
