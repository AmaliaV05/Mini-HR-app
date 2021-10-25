using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.ViewModels
{
    public class CompanyWithEmployeesViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public List<EmployeeWithDetailsViewModel> Employees { get; set; }
    }
}
