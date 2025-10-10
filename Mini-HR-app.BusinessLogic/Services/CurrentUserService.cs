using Microsoft.AspNetCore.Http;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.BusinessLogic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return null;
        }

        public List<string> GetCurrentUserRoles()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            }

            return new List<string>();
        }

        public string GetCurrentUserMainRole()
        {
            var userRoles = GetCurrentUserRoles();
            var userRole = userRoles.FirstOrDefault();

            return userRole;
        }

        public CurrentUser GetCurrentUser() => new()
        {
            Id = GetCurrentUserId(),
            MainRole = GetCurrentUserMainRole(),
            Roles = GetCurrentUserRoles()
        };
    }
}
