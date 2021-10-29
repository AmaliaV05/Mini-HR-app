using Mini_HR_app.Models;

namespace Mini_HR_app.Data.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
