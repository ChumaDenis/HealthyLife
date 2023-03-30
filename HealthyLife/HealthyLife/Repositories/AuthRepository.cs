using HealthyLife.Contexts;
using HealthyLife.Models;
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

        public bool CheckUser(string UserName)
        {
            return _dbContext.Users.Any(x=>x.UserName==UserName);
        }
        public bool CheckUser(UserDTO userDTO)
        {
             User? user = _dbContext.Users.FirstOrDefault(e => e.UserEmail == userDTO.UserEmail);
             if (user != null)
             {
                return VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt);
             }
             return false;
            
        }

        public void AddUser(UserDTO request)
        {
            try
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = new User() {UserName=request.UserName, PasswordSalt = passwordSalt,  PasswordHash= passwordHash, UserEmail=request.UserEmail};
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

        public User GetUserDetails(string id)
        {
            try
            {
                User? user = _dbContext.Users.Find(id);
                if (user != null)
                {
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

        public void UpdateUser(User user)
        {
            try
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }



        public bool CheckToken(string value)
        {
            return _dbContext.Tokens.Any(e => e.Value==value && e.CreateDate>=DateTime.Now);
        }


        public string CreateToken(UserDTO userDTO)
        {
            User? user = _dbContext.Users.First(e => e.UserEmail == userDTO.UserEmail);
            if (VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.UserEmail)
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                Token token1= new Token() { Value= jwt, UserId=user.Id };


                return jwt;
            }
            return "error";
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
