using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mini_HR_app.Data;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SymmetricSecurityKey _key;
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context, SymmetricSecurityKey key)
        {
            _context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Our super secret key@123"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            if (await _context.AppUsers.AnyAsync(x => x.Username == registerUserRequest.UserName.ToLower()))
            {
                return BadRequest("Username is taken!");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerUserRequest.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserRequest.Password)),
                PasswordSalt = hmac.Key
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInUser>> Login(LoginUserRequest loginUserRequest)
        {
            var user = await _context.AppUsers
                .SingleOrDefaultAsync(x => x.Username == loginUserRequest.Username);

            if (user is null)
            {
                return Unauthorized("Username does not exist");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUserRequest.Password));

            if(computedHash.Length != user.PasswordHash.Length)
            {
                return Unauthorized("Invalid Password");
            }

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordSalt[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            var loggedInUser = new LoggedInUser
            {
                Username = user.Username,
                Token = CreateToken(user)
            };

            return loggedInUser;
        }

        private string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer = "https://localhost:5001"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}