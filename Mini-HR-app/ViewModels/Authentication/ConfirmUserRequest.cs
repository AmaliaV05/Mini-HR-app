namespace Mini_HR_app.ViewModels.Authentication
{
    public class ConfirmUserRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
