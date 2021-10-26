using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;
        private User _user;

        public TokenService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<JwtSecurityToken> CreateToken(LoginUserRequest loginUserRequest)
        {
            if (!(await ValidateUser(loginUserRequest)))
            {
                return null;
            }

            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions( claims);
            
            return tokenOptions;
        }

        private async Task<bool> ValidateUser(LoginUserRequest loginUserRequest)
        {
            _user = await _userManager.FindByNameAsync(loginUserRequest.Username);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, loginUserRequest.Password));
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim("name", _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            claims.AddRange(roles.Select(role => new Claim("role", role)));

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(List<Claim> claims)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            int expiryInMinutes = Convert.ToInt32(jwtSettings.GetSection("expires").Value);

            var signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                claims: claims,
                signingCredentials: signingCredentials
                );
        }
    }
}
