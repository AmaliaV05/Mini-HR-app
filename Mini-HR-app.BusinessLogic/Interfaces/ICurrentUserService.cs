using Mini_HR_app.BusinessLogic.Models;
using System.Collections.Generic;

namespace Mini_HR_app.BusinessLogic.Interfaces
{
    public interface ICurrentUserService
    {
        string GetCurrentUserId();
        string GetCurrentUserMainRole();
        List<string> GetCurrentUserRoles();
        CurrentUser GetCurrentUser();
    }
}