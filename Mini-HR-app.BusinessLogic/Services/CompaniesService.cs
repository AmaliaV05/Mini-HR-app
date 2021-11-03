using Microsoft.EntityFrameworkCore;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.Data;
using Mini_HR_app.Exceptions;
using Mini_HR_app.Helpers;
using Mini_HR_app.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public class CompaniesService : ICompaniesService
    {
        public readonly ApplicationDbContext _context;
        public readonly IEmailService _emailService;

        public CompaniesService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<PagedList<Company>> GetActiveCompanies(CompanyParams companyParams)
        {
            var query = _context.Companies.Where(c => c.Status == true).AsQueryable();

            //filter by range of years
            if (!companyParams.ValidYearRange)
            {
                throw new GetCompanyException("Invalid year range");
            }
            query = query.Where(f => f.DateOfEstablishment.Year <= companyParams.MaxYear &&
                            f.DateOfEstablishment.Year >= companyParams.MinYear);

            //search by title
            if (!string.IsNullOrEmpty(companyParams.CompanyName))
            {
                query = query.Where(f => f.CompanyName.ToLower().Contains(companyParams.CompanyName.ToLower()));
            }

            //sort by company name/date of establishment/none
            ApplySort(query, companyParams.Property);

            return await PagedList<Company>.CreateAsync(query.AsNoTracking(),
                companyParams.PageNumber, companyParams.PageSize);
        }

        private static IQueryable<Company> ApplySort(IQueryable<Company> query, string property)
        {
            return property switch
            {
                "Name" => query.OrderBy(p => p.CompanyName),
                "Date" => query.OrderBy(p => p.DateOfEstablishment),
                _ => query,
            };
        }

        public async Task<Company> GetCompanyDetails(int idCompany)
        {
            var company = await _context.Companies
                .Where(c => c.Status == true && c.Id == idCompany)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                throw new GetCompanyException($"Company with id: {idCompany} does not exist");
            }

            return company;
        }

        public async Task<Company> GetActiveEmployees(int idCompany)
        {
            await CheckCompanyIsActive(idCompany);

            return await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .ThenInclude(c => c.Employee)
                .AsSplitQuery()
                .FirstAsync();
        }

        public async Task<Company> GetEmployeeDetails(int idCompany, int idEmployee)
        {
            await CheckCompanyIsActive(idCompany);

            var employee = await _context.Companies
               .Where(c => c.Id == idCompany)
               .Include(c => c.Employees.Where(e => e.Id == idEmployee))
               .ThenInclude(e => e.CompanyEmployees.Where(e => e.Status == true))
               .AsSplitQuery()
               .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new GetEmployeeException($"Employee with id: {idEmployee} does not exist");
            }

            return employee;
        }

        public async Task<Employee> FindEmployeeId(Employee employee)
        {
            var e = await _context.Employees.Where(e => e.Ssn == employee.Ssn).FirstAsync();

            return e;
        }

        private async Task CheckCompanyIsActive(int idCompany)
        {
            var company = await _context.Companies
               .Where(c => c.Status == true && c.Id == idCompany)
               .FirstOrDefaultAsync();

            if (company == null)
            {
                throw new GetCompanyException($"Company with id: {idCompany} does not exist");
            }
        }        

        public async Task PutCompanyDetails(int idCompany, Company company)
        {
            if (idCompany != company.Id)
            {
                throw new PutCompanyException($"Company id: {company.Id} does not match input id: {idCompany}");
            }

            if (!await _context.Companies.AnyAsync(e => e.Id == idCompany))
            {
                throw new PutCompanyException($"Company id: {company.Id} does not exist");
            }

            company.Status = true;

            _context.Entry(company).State = EntityState.Modified;                   
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task PutEmployeeDetails(int idCompany, int idEmployee, Employee employee)
        {
            if (idEmployee != employee.Id)
            {
                throw new PutEmployeeException($"Employee id: {employee.Id} does not match input id: {idEmployee}");
            }

            if (!await _context.Employees.AnyAsync(e => e.Id == idEmployee))
            {
                throw new PutEmployeeException($"Employee id: {employee.Id} does not exist");
            }

            _context.Entry(employee).State = EntityState.Modified;
        }

        public async Task PutCompanyStatusToInactive(int idCompany)
        {
            await CheckCompanyIsActive(idCompany);

            var company = await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .FirstOrDefaultAsync();

            if (company.CompanyEmployees.Count != 0)
            {
                throw new PutCompanyException($"Company still has {company.CompanyEmployees.Count} employees left");
            }

            company.Status = false;

            _context.Entry(company).Property(x => x.Status).IsModified = true;
        }

        public async Task PutEmployeeStatusToInactive(int idCompany, int idEmployee)
        {
            await CheckCompanyIsActive(idCompany);

            var company = _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .Include(c => c.Employees.Where(e => e.Id == idEmployee))
                .AsSplitQuery()
                .FirstOrDefault();

            if (company == null)
            {
                throw new PutEmployeeException($"Employee with id: {idEmployee} does not exist");
            }

            var employee = company.CompanyEmployees.FirstOrDefault();

            employee.Status = false;

            _context.Entry(employee).Property(x => x.Status).IsModified = true;
        }

        public async Task PostCompany(Company company)
        {
            var checkCompany = await _context.Companies
                .Where(c => c.FiscalCode == company.FiscalCode)
                .FirstOrDefaultAsync();

            if (checkCompany != null)
            {
                throw new PostCompanyException($"Company with fiscal code {checkCompany.FiscalCode} already exists");
            }

            company.Status = true;

            await _context.Companies.AddAsync(company);
        }

        public async Task PostEmployeeForCompany(int idCompany, Employee employee)
        {
            var company = await _context.Companies
                .Where(c => c.Id == idCompany && c.Status == true)
                .Include(c => c.CompanyEmployees)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                throw new PostEmployeeException($"Company with id: {idCompany} does not exist");
            }

            var existingEmployee = await _context.Employees
                .Where(p => p.Ssn == employee.Ssn)
                .FirstOrDefaultAsync();

            if (existingEmployee == null)
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            else throw new PostEmployeeException($"Employee already exists");

            var existingCompany = await _context.Companies
                .Include(c => c.Employees)
                .SingleAsync(c => c.Id == idCompany);

            if (company.CompanyEmployees.Count == 0)
            {
                await _context.CompanyEmployee.AddAsync(new CompanyEmployee
                {
                    Company = existingCompany,
                    Employee = employee,
                    Status = true
                });
            }
            else
            {
                existingCompany.CompanyEmployees.Add(new CompanyEmployee
                {
                    Company = existingCompany,
                    Employee = employee,
                    Status = true
                });
            }            
        }        
    }
}
