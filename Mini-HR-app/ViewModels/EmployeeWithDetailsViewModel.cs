using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.ViewModels
{
    public class EmployeeWithDetailsViewModel
    {
        public int Id { get; set; }
        public PersonViewModel Person { get; set; }
    }
}
