using Microsoft.AspNetCore.Identity;

namespace Mini_HR_app.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
