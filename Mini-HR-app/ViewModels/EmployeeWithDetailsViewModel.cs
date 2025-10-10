using System;

namespace Mini_HR_app.ViewModels
{
    public class EmployeeWithDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Ssn { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
    }
}
