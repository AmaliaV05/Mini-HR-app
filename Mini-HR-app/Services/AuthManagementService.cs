using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app.Data;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels.Authentication;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public class AuthManagementService : IAuthManagementService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthManagementService(ApplicationDbContext context, UserManager<User> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            var user = new User
            {
                FirstName = registerUserRequest.FirstName,
                LastName = registerUserRequest.LastName,
                UserName = registerUserRequest.UserName,
                Email = registerUserRequest.Email,
                PhoneNumber = registerUserRequest.PhoneNumber
            };

            var serviceResponse = new ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>();
            var result = await _userManager.CreateAsync(user, registerUserRequest.Password);
            
            if (result.Succeeded)
            {
                serviceResponse.ResponseOk = new RegisterResponse { ConfirmationToken = user.SecurityStamp };
                return serviceResponse;
            }

            await _userManager.AddToRoleAsync(user, "Member");

            serviceResponse.ResponseError = result.Errors;
            return serviceResponse;
        }

        public async Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest)
        {
            var toConfirm = _context.Users
                .Where(u => u.Email == confirmUserRequest.Email && u.PhoneNumber == confirmUserRequest.PhoneNumber &&
                u.SecurityStamp == confirmUserRequest.ConfirmationToken)
                .FirstOrDefault();
            
            if (toConfirm != null)
            {
                toConfirm.EmailConfirmed = true;
                toConfirm.PhoneNumberConfirmed = true;
                _context.Entry(toConfirm).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginUserRequest loginUserRequest)
        {
            var serviceResponse = new ServiceResponse<LoginResponse, string>();
            
            var user = await _userManager.FindByNameAsync(loginUserRequest.Username);            

            if (user != null && await _userManager.CheckPasswordAsync(user, loginUserRequest.Password))
            {
                var token = await _tokenService.CreateToken(loginUserRequest);

                serviceResponse.ResponseOk = new LoginResponse { 
                    Username = loginUserRequest.Username,
                    Token = new JwtSecurityTokenHandler().WriteToken(token), 
                    Expiration = token.ValidTo 
                };
            }

            return serviceResponse;
        }
    }
}