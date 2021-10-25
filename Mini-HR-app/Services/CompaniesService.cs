using Microsoft.EntityFrameworkCore;
using Mini_HR_app.Data;
using Mini_HR_app.Helpers;
using Mini_HR_app.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Services
{
    public class CompaniesService : ICompaniesService
    {
        public readonly ApplicationDbContext _context;

        public CompaniesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Company>> GetActiveCompanies(CompanyParams companyParams)
        {
            var query = _context.Companies.Where(c => c.Status == true).AsQueryable();

            if (companyParams.MinYearOfEstablishment != null && companyParams.MaxYearOfEstablishment != null)
            {
                if (!companyParams.ValidYearRange)
                {
                    return null;
                }

                query = query.Where(f => f.DateOfEstablishment.Year <= companyParams.MaxYearOfEstablishment &&
                                f.DateOfEstablishment.Year >= companyParams.MinYearOfEstablishment);
            }
            else if (companyParams.MinYearOfEstablishment != null && companyParams.MaxYearOfEstablishment == null)
            {
                query = query.Where(f => f.DateOfEstablishment.Year >= companyParams.MinYearOfEstablishment);
            }
            else if (companyParams.MinYearOfEstablishment == null && companyParams.MaxYearOfEstablishment != null)
            {
                query = query.Where(f => f.DateOfEstablishment.Year <= companyParams.MaxYearOfEstablishment);
            }

            return await PagedList<Company>.CreateAsync(query.AsNoTracking(),
                companyParams.PageNumber, companyParams.PageSize);
        }

        public async Task<Company> GetCompanyDetails(int idCompany)
        {
            var company = await _context.Companies
                .Where(c => c.Status == true && c.Id == idCompany)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return null;
            }

            return company;
        }

        public async Task<Company> GetActiveEmployees(int idCompany)
        {
            var company = await _context.Companies
                .Where(c => c.Status == true && c.Id == idCompany)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return null;
            }

            return await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .ThenInclude(c => c.Employee)
                .ThenInclude(e => e.Person)
                .AsSplitQuery()
                .FirstAsync();
        }

        public async Task<Company> GetEmployeeDetails(int idCompany, int idEmployee)
        {  
            return await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.Employees.Where(e => e.Id == idEmployee))
                .ThenInclude(e => e.Person)
                .AsSplitQuery()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckCompanyIsActive(int idCompany)
        {
            var company = await _context.Companies
               .Where(c => c.Status == true && c.Id == idCompany)
               .FirstOrDefaultAsync();

            if (company == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckEmployeeIsActive(int idEmployee)
        {
            var employee = await _context.Employees
               .Where(e => e.Id == idEmployee)
               .Include(e => e.CompanyEmployees.Where(e => e.Status == true))
               .FirstOrDefaultAsync();

            if (employee == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> PutCompanyDetails(int idCompany, Company company)
        {
            company.Status = true;

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CompanyExists (idCompany))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> PutEmployeeDetails(int idCompany, int idEmployee, Person person)
        {
            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CompanyExists(idCompany) || !await EmployeeExists(idEmployee))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> PutCompanyStatusToInactive(int idCompany)
        {
            var company = await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .FirstOrDefaultAsync();

            if (company != null)
            {
                return false;
            }

            company.Status = false;

            _context.Entry(company).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PutEmployeeStatusToInactive(int idCompany, int idEmployee)
        {
            var company = _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .Include(c => c.Employees.Where(e => e.Id == idEmployee))
                .AsSplitQuery()
                .FirstOrDefault();

            if (company == null)
            {
                return false;
            }

            var employee = company.CompanyEmployees.FirstOrDefault();

            employee.Status = false;

            _context.Entry(employee).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PostCompany(Company company)
        {
            var checkCompany = await _context.Companies
                .Where(c => c.FiscalCode == company.FiscalCode)
                .FirstOrDefaultAsync();

            if (checkCompany != null)
            {
                return false;
            }

            company.Status = true;

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PostEmployeeForCompany(int idCompany, Person person)
        {
            var company = await _context.Companies
                .Where(c => c.Id == idCompany && c.Status == true)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return false;
            }

            var existingPerson = await _context.People
                .Where(p => p.Ssn == person.Ssn)
                .FirstOrDefaultAsync();

            if (existingPerson == null)
            {
                existingPerson = person;

                await _context.People.AddAsync(existingPerson);
                await _context.SaveChangesAsync();
            }

            var employee = new Employee
            {
                PersonId = existingPerson.Id
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            var existingEmployee = await _context.Employees
                .SingleAsync(e => e.Person.Ssn == existingPerson.Ssn);

            var existingCompany = await _context.Companies
                .Include(c => c.Employees)
                .SingleAsync(c => c.Id == idCompany);

            company.CompanyEmployees.Add(new CompanyEmployee
            {
                Company = existingCompany,
                Employee = existingEmployee,
                Status = true
            });
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CompanyExists(int id)
        {
            return await _context.Companies.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }
    }
}
