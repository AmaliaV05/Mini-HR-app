using Mini_HR_app.Data.Models;
using System;
using System.Collections.Generic;

namespace Mini_HR_app.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Ssn { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public List<Company> Companies { get; set; }
        public List<CompanyEmployee> CompanyEmployees { get; set; }
        public Photo Photo { get; set; }
    }
}
