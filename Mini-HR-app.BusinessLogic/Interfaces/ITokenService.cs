using Mini_HR_app.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateToken(LoginUserRequest loginUserRequest);
    }
}
