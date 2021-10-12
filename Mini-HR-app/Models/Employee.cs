using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
