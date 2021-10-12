using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        /// Retrieve company details
        /// </summary>
        /// <returns>Company details</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyViewModel>>> GetCompany()
        {
            return await _context.Company
                .Select(c => _mapper.Map<CompanyViewModel>(c))
                .ToListAsync();
        }

        /// <summary>
        /// Create company details
        /// </summary>
        /// <param name="companyViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CompanyViewModel>> PostCompany(CompanyViewModel companyViewModel)
        {
            var checkCompany = await _context.Company.ToListAsync();

            if (checkCompany.Count == 1)
            {
                return BadRequest("Company details already exist!");
            }

            var company = _mapper.Map<Company>(companyViewModel);  

            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        /// <summary>
        /// Creates new employee entry
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="employeeWithDetails"></param>
        /// <returns></returns>
        [HttpPost("{idCompany}/Employee")]
        public async Task<ActionResult> PostEmployeeForCompany(int idCompany, EmployeeWithDetailsViewModel employeeWithDetails)
        {
            var company = await _context.Company
                .Where(c => c.Id == idCompany)
                .Include(c => c.Employees)
                .FirstOrDefaultAsync();
            
            if (company == null)
            {
                return NotFound("Company details do not exist");
            }

            var personViewModel = employeeWithDetails.Person;

            var person = _mapper.Map<Person>(personViewModel);

            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            var findPerson = _context.Person
                .Where(p => p.Ssn == employeeWithDetails.Person.Ssn)
                .First();

            var employee = new Employee
            {
                PersonId = findPerson.Id
            };

            company.Employees.Add(employee);
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("New employee entry was added.");
        }
    }
}
