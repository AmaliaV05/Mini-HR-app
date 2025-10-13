using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.Services;
using Mini_HR_app.ViewModels.Authentication;
using System.Threading.Tasks;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManagementService _authenticationService;

        public AccountController(IAccountManagementService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers a new Manager or Member in the system
        /// </summary>
        /// <remarks>
        /// Allows an Admin to register a new Manager in the system, and Managers to register 
        /// users with 'Member' role. Only Admins and Managers can access this endpoint
        /// </remarks>
        /// <param name="registerUserRequest"></param>
        /// <returns>Register response</returns>
        /// <response code="200">New user registered successfully</response>
        /// <response code="400">Invalid registration request</response>
        [HttpPost("register")]
        [Authorize(Policy = "Admin_Manager")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            // TODO: add check for member role (403 status)
            var registerServiceResult = await _authenticationService.RegisterUser(registerUserRequest);
            if (registerServiceResult.ResponseError != null)
            {
                return BadRequest(registerServiceResult.ResponseError);
            }

            return Ok(registerServiceResult.ResponseOk);
        }

        // TODO: remove endpoint
        /// <summary>
        /// Confirms registration of a new user
        /// </summary>
        /// <param name="confirmUserRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Allows users to log in
        /// </summary>
        /// <param name="loginUserRequest"></param>
        /// <returns>Login response</returns>
        /// <response code="200">User successfully logged in</response>
        /// <response code="401">User cannot be authenticated</response>
        [AllowAnonymous]
        [HttpPost("login")]        
        public async Task<ActionResult<LoginResponse>> Authenticate(LoginUserRequest loginUserRequest)
        {
            var serviceResult = await _authenticationService.LoginUser(loginUserRequest);

            if (serviceResult.ResponseOk != null)
            {
                return Ok(serviceResult.ResponseOk);
            }

            return Unauthorized(serviceResult.ResponseError);
        }
    }
}
