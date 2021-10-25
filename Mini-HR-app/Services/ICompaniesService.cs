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
        Task<bool> PutCompanyDetails(int idCompany, Company company);
        Task<bool> PutEmployeeDetails(int idCompany, int idEmployee, Person person);
        Task<bool> PutCompanyStatusToInactive(int idCompany);
        Task<bool> PutEmployeeStatusToInactive(int idCompany, int idEmployee);
        Task<bool> PostCompany(Company company);
        Task<bool> PostEmployeeForCompany(int idCompany, Person person);
        Task<bool> CheckCompanyIsActive(int idCompany);
        Task<bool> CheckEmployeeIsActive(int idEmployee);
    }
}
