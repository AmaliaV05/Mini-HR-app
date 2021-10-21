using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.Services;
using Mini_HR_app.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAuthManagementService _authenticationService;

        public AccountController(IAuthManagementService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            var registerServiceResult = await _authenticationService.RegisterUser(registerUserRequest);
            if (registerServiceResult.ResponseError != null)
            {
                return BadRequest(registerServiceResult.ResponseError);
            }

            return Ok(registerServiceResult.ResponseOk);
        }

        [HttpPost("confirm")]
        public async Task<ActionResult> ConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            var serviceResult = await _authenticationService.ConfirmUserRequest(confirmUserRequest);
            
            if (serviceResult)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Authenticate(LoginUserRequest loginUserRequest)
        {
            var serviceResult = await _authenticationService.LoginUser(loginUserRequest);

            if (serviceResult.ResponseOk != null)
            {
                return Ok(serviceResult.ResponseOk);
            }

            return Unauthorized();
        }
    }
}
