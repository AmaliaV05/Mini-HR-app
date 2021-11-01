using Microsoft.AspNetCore.Http;
using Mini_HR_app.Data.Models;
using Mini_HR_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Interfaces
{
    public interface IEmployeesService
    {
        Task AddPhoto(int idEmployee, IFormFile file);
        Task<Employee> GetEmployeePhoto(int idEmployee);
        Task DeletePhoto(int idEmployee);
        Task PostEmployeeContact(int idEmployee, Contact contact);
        Task<bool> SaveChangesAsync();
    }
}
