using Mini_HR_app.Models;

namespace Mini_HR_app.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string MSTeams { get; set; }
        public string Email { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public Employee Employee { get; set; }
    }
}
