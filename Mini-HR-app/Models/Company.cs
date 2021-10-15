using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string FiscalCode { get; set; }
        public string RegistryNo { get; set; }
        public string Euid { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public string NaceCode { get; set; }
        public string Activity { get; set; }
        public string ActivityDescription { get; set; }
        public bool Status { get; set; }
        public List<Employee> Employees { get; set; }
        public List<CompanyEmployee> CompanyEmployees { get; set; }
    }
}
