using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Models
{
    public class Employee
    {
        public Company Company { get; set; }
        public Person Person { get; set; }
        public bool Active { get; set; }
    }
}
