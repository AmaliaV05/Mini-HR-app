using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Ssn { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
