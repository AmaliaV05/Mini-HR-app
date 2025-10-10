using System.Collections.Generic;

namespace Mini_HR_app.BusinessLogic.Models
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string MainRole { get; set; }
        public List<string> Roles { get; set; }
    }
}
