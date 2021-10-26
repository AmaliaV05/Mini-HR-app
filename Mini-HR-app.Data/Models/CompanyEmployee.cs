using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Models
{
    public class CompanyEmployee
    {
        public Company Company { get; set; }
        public Employee Employee { get; set; }
        public bool Status { get; set; }
    }
}
