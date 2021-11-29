using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.Extensions;
using Mini_HR_app.Helpers;
using Mini_HR_app.Models;
using Mini_HR_app.Services;
using Mini_HR_app.ViewModels;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private ICompaniesService _companyService;
        private readonly IMapper _mapper;

        public CompaniesController(ICompaniesService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all companies' details
        /// </summary>
        /// <param name="companyParams"></param>
        /// <returns></returns>
        [HttpGet("Active-Status")]
        public async Task<ActionResult<IEnumerable<CompanyViewModel>>> GetCompanies([FromQuery] CompanyParams companyParams)
        {
            var companies = await _companyService.GetActiveCompanies(companyParams);

            Response.AddPaginationHeader(companies.CurrentPage, companies.PageSize, companies.TotalCount, companies.TotalPages);

            return companies
                .Select(c => _mapper.Map<CompanyViewModel>(c))
                .ToList();
        }

        /// <summary>
        /// Get details of a company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns></returns>
        [HttpGet("{idCompany}")]
        public async Task<ActionResult<CompanyViewModel>> GetCompanyDetails(int idCompany)
        {
            var company = await _companyService.GetCompanyDetails(idCompany);

            return _mapper.Map<CompanyViewModel>(company);
        }

        /// <summary>
        /// Gets a list of active employees with personal details from a company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns>List of employees</returns>
        [HttpGet("{idCompany}/Employees-Active-Status")]
        /*[Authorize(Roles = "Manager")]*/
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetActiveEmployees(int idCompany)
        {
            var company = await _companyService.GetActiveEmployees(idCompany);

            return _mapper.Map<CompanyWithEmployeesViewModel>(company);
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
            var employees = await _companyService.GetEmployeeDetails(idCompany, idEmployee);

            return _mapper.Map<CompanyWithEmployeesViewModel>(employees);
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
            _ = new Company();
            Company company;

            company = _mapper.Map<Company>(companyViewModel);

            await _companyService.PutCompanyDetails(idCompany, company);

            await _companyService.SaveChangesAsync();

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
            var employee = _mapper.Map<Employee>(employeeWithDetailsViewModel);

            await _companyService.PutEmployeeDetails(idCompany, idEmployee, employee);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Changes the status of the company to inactive
        /// </summary>
        /// <param name="idCompany"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}/Change-Status-To-Inactive")]
        public async Task<ActionResult> PutCompanyStatusToInactive(int idCompany)
        {
            await _companyService.PutCompanyStatusToInactive(idCompany);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Changes the status of employee to inactive, meaning the person is no longer an employee of the company
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpPut("{idCompany}/Employee-Change-Status-To-Inactive/{idEmployee}")]
        public async Task<ActionResult> PutEmployeeStatusToInactive(int idCompany, int idEmployee)
        {
            await _companyService.PutEmployeeStatusToInactive(idCompany, idEmployee);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create company details
        /// </summary>
        /// <param name="companyViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostCompany(CompanyViewModel companyViewModel)
        {
            var company = _mapper.Map<Company>(companyViewModel);

            await _companyService.PostCompany(company);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates new employee entry
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="employeeWithDetailsViewModel"></param>
        /// <returns></returns>
        [HttpPost("{idCompany}/Employee")]
        public async Task<ActionResult> PostEmployeeForCompany(int idCompany, EmployeeWithDetailsViewModel employeeWithDetailsViewModel)
        {
            var employee = _mapper.Map<Employee>(employeeWithDetailsViewModel);

            await _companyService.PostEmployeeForCompany(idCompany, employee);

            await _companyService.SaveChangesAsync();

            var employeeToReturn = await _companyService.FindEmployeeId(employee);
            var employeeToReturnViewModel = _mapper.Map<EmployeeWithDetailsViewModel>(employeeToReturn);

            return CreatedAtAction("GetEmployeeDetails", new { idC = idCompany, idEmployee = employeeToReturn.Id }, employeeToReturnViewModel);
        }
    }
}