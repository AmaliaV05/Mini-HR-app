using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app.Data;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CompaniesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all companies' details
        /// </summary>
        /// <returns>Company details</returns>
        [HttpGet("Active-Status")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CompanyViewModel>>> GetCompanies()
        {
            return await _context.Companies
                .Where(c => c.Status == true)
                .Select(c => _mapper.Map<CompanyViewModel>(c))
                .ToListAsync();
        }

        /// <summary>
        /// Get details of a company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns></returns>
        [HttpGet("{idCompany}")]
        public async Task<ActionResult<CompanyViewModel>> GetCompanyDetails(int idCompany)
        {
            var company = await _context.Companies.FindAsync(idCompany);

            if (company == null)
            {
                return NotFound();
            }

            var companyViewModel = _mapper.Map<CompanyViewModel>(company);
            return companyViewModel;
        }

        /// <summary>
        /// Gets a list of active employees with personal details from a company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns>List of employees</returns>
        [HttpGet("{idCompany}/Employees-Active-Status")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetActiveEmployees(int idCompany)
        {
            var company = await _context.Companies.FindAsync(idCompany);

            if (company == null)
            {
                return NotFound("The company does not exist");
            }

            return await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .ThenInclude(c => c.Employee)
                .ThenInclude(e => e.Person)
                .AsSplitQuery()
                .Select(e => _mapper.Map<CompanyWithEmployeesViewModel>(e))
                .FirstAsync();
        }

        /// <summary>
        /// Gets the details of an employee from a company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpGet("{idCompany}/Employee/{idEmployee}")]
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetEmployeeDetails(int idCompany, int idEmployee)
        {
            var company = await _context.Companies.FindAsync(idCompany);

            if (company == null)
            {
                return NotFound("Company does not exist");
            }

            var employee = await _context.Employees.FindAsync(idEmployee);

            if (employee == null)
            {
                return NotFound("Employee does not exist");
            }

            company = await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.Employees.Where(e => e.Id == idEmployee))
                .ThenInclude(e => e.Person)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            var employeeViewModel = _mapper.Map<CompanyWithEmployeesViewModel>(company);

            return employeeViewModel;
        }

        /// <summary>
        /// Updates company details
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="companyViewModel"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}")]
        public async Task<ActionResult> PutCompanyDetails(int idCompany, CompanyViewModel companyViewModel)
        {
            var company = _mapper.Map<Company>(companyViewModel);

            if (company.Id != idCompany)
            {
                return BadRequest("Company id does not match the input id");
            }

            company.Status = true;

            _context.Entry(company).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(idCompany))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Updates employee details
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <param name="employeeWithDetailsViewModel"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}/Employee/{idEmployee}")]
        public async Task<ActionResult> PutEmployeeDetails(int idCompany, int idEmployee, EmployeeWithDetailsViewModel employeeWithDetailsViewModel)
        {
            var company = await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.Employees)
                .FirstOrDefaultAsync();

            var person = _mapper.Map<Person>(employeeWithDetailsViewModel.Person);

            if (idEmployee != employeeWithDetailsViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(idCompany) || !EmployeeExists(idEmployee))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Changes the status of the company to inactive
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}/Change-Status-To-Inactive")]
        public async Task<ActionResult<CompanyViewModel>> PutCompanyStatusToInactive(int idCompany)
        {
            var company = _context.Companies
                .Where(c => c.Id == idCompany)
                .FirstOrDefault();

            if (company == null)
            {
                return BadRequest("The company does not exist");
            }

            /*var checkCompanyEmployees = await _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .FirstOrDefaultAsync();

            Console.WriteLine(checkCompanyEmployees.CompanyEmployees.Count);
            Console.WriteLine(checkCompanyEmployees);

            if (checkCompanyEmployees != null)
            {
                return BadRequest("The company still has employees");
            }*/

            company.Status = false;

            _context.Entry(company).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Changes the status of employee to inactive, meaning the person is no longer an employee of the company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}/Employee-Change-Status-To-Inactive/{idEmployee}")]
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> PutEmployeeStatusToInactive(int idCompany, int idEmployee)
        {
            var company = _context.Companies
                .Where(c => c.Id == idCompany)
                .Include(c => c.CompanyEmployees.Where(e => e.Status == true))
                .Include(c => c.Employees.Where(e => e.Id == idEmployee))
                .AsSplitQuery()
                .FirstOrDefault();

            if (company == null)
            {
                return BadRequest("The employee is not active at the chosen company");
            }

            var employee = company.CompanyEmployees.FirstOrDefault();

            employee.Status = false;

            _context.Entry(employee).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Create company details
        /// </summary>
        /// <param name="companyViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CompanyViewModel>> PostCompany(CompanyViewModel companyViewModel)
        {
            var checkCompany = await _context.Companies
                .Where(c => c.FiscalCode == companyViewModel.FiscalCode)
                .FirstOrDefaultAsync();

            if (checkCompany != null)
            {
                return BadRequest("Company already exist");
            }            

            var company = _mapper.Map<Company>(companyViewModel);

            company.Status = true;

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        /// <summary>
        /// Creates new employee entry
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="personViewModel"></param>
        /// <returns></returns>
        [HttpPost("{idCompany}/Employee")]
        public async Task<ActionResult> PostEmployeeForCompany(int idCompany, PersonViewModel personViewModel)
        {
            var company = await _context.Companies
                .Where(c => c.Id == idCompany)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return NotFound("Company details do not exist");
            }

            var findPerson = _context.People
                .Where(p => p.Ssn == personViewModel.Ssn)
                .FirstOrDefault();

            if (findPerson == null)
            {
                var person = _mapper.Map<Person>(personViewModel);

                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();
            }            

            var existingPerson = _context.People
                .Single(p => p.Ssn == personViewModel.Ssn);
            
            var employee = new Employee
            {
                PersonId = existingPerson.Id
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            var existingEmployee = _context.Employees
                .Single(e => e.Person.Ssn == existingPerson.Ssn);

            var existingCompany = _context.Companies
                .Include(c => c.Employees)
                .Single(c => c.Id == idCompany);

            company.CompanyEmployees.Add(new CompanyEmployee
            {
                Company = existingCompany,
                Employee = existingEmployee,
                Status = true
            });
            await _context.SaveChangesAsync();

            return Ok("New employee entry was added");
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
