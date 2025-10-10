using Microsoft.AspNetCore.Identity;
using Mini_HR_app.ViewModels.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public interface IAuthManagementService
    {
        Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterUserRequest registerUserRequest);
        Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest);
        Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginUserRequest loginUserRequest);
    }
}
