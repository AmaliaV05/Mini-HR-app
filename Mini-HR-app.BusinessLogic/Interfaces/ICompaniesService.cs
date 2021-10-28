using Mini_HR_app.Helpers;
using Mini_HR_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public interface ICompaniesService
    {
        Task<PagedList<Company>> GetActiveCompanies(CompanyParams companyParams);
        Task<Company> GetCompanyDetails(int idCompany);
        Task<Company> GetActiveEmployees(int idCompany);
        Task<Company> GetEmployeeDetails(int idCompany, int idEmployee);
        Task PutCompanyDetails(int idCompany, Company company);
        Task PutEmployeeDetails(int idCompany, int idEmployee, Employee employee);
        Task PutCompanyStatusToInactive(int idCompany);
        Task PutEmployeeStatusToInactive(int idCompany, int idEmployee);
        Task PostCompany(Company company);
        Task PostEmployeeForCompany(int idCompany, Employee employee);
        Task<bool> SaveChangesAsync();
    }
}
