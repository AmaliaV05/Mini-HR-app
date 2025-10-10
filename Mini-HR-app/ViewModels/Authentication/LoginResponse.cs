using System;

namespace Mini_HR_app.ViewModels.Authentication
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
