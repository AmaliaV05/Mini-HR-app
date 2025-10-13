using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _companyService;
        private readonly IMapper _mapper;

        public CompaniesController(ICompaniesService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all companies
        /// </summary>
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="companyParams"></param>
        /// <returns>List of companies</returns>
        /// <response code="200">List of companies successfully retrieved</response>
        [HttpGet("Active-Status")]
        [Authorize(Policy = "Admin_Manager_Member")]
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
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <returns>Company details</returns>
        /// <response code="200">Company details successfully retrieved</response>
        [HttpGet("{idCompany}")]
        [Authorize(Policy = "Admin_Manager_Member")]
        public async Task<ActionResult<CompanyViewModel>> GetCompanyDetails(int idCompany)
        {
            var company = await _companyService.GetCompanyDetails(idCompany);

            return _mapper.Map<CompanyViewModel>(company);
        }

        /// <summary>
        /// Gets a list of active employees with personal details from a company
        /// </summary>
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <returns>List of employees</returns>
        /// <response code="200">Active employees of a company successfully retrieved</response>
        [HttpGet("{idCompany}/Employees-Active-Status")]
        [Authorize(Policy = "Admin_Manager_Member")]
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetActiveEmployees(int idCompany)
        {
            var company = await _companyService.GetActiveEmployees(idCompany);

            return _mapper.Map<CompanyWithEmployeesViewModel>(company);
        }

        /// <summary>
        /// Gets the details of an employee from a company
        /// </summary>
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <returns>Employee details</returns>
        /// <response code="200">Active employees details successfully retrieved</response>
        [HttpGet("{idCompany}/Employee/{idEmployee}")]
        [Authorize(Policy = "Admin_Manager_Member")]
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetEmployeeDetails(int idCompany, int idEmployee)
        {
            var employees = await _companyService.GetEmployeeDetails(idCompany, idEmployee);

            return _mapper.Map<CompanyWithEmployeesViewModel>(employees);
        }

        /// <summary>
        /// Updates company details
        /// </summary>
        /// <remarks>
        /// Only Admin and Managers can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <param name="companyViewModel"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPut("{idCompany}")]
        [Authorize(Policy = "Admin_Manager")]
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
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <param name="employeeWithDetailsViewModel"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPut("{idCompany}/Employee/{idEmployee}")]
        [Authorize(Policy = "Admin_Manager_Member")]
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
        /// <remarks>
        /// Only Admin and Managers can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPut("{idCompany}/Change-Status-To-Inactive")]
        [Authorize(Policy = "Admin_Manager")]
        public async Task<ActionResult> PutCompanyStatusToInactive(int idCompany)
        {
            await _companyService.PutCompanyStatusToInactive(idCompany);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Changes the status of employee to inactive, meaning the person is no longer an employee of the company
        /// </summary>
        /// <remarks>
        /// Only Admin and Managers can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <param name="idEmployee"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPut("{idCompany}/Employee-Change-Status-To-Inactive/{idEmployee}")]
        [Authorize(Policy = "Admin_Manager")]
        public async Task<ActionResult> PutEmployeeStatusToInactive(int idCompany, int idEmployee)
        {
            await _companyService.PutEmployeeStatusToInactive(idCompany, idEmployee);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create company details
        /// </summary>
        /// <remarks>
        /// Only Admin and Managers can access this endpoint
        /// </remarks>
        /// <param name="companyViewModel"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPost]
        [Authorize(Policy = "Admin_Manager")]
        public async Task<ActionResult> PostCompany(CompanyViewModel companyViewModel)
        {
            var company = _mapper.Map<Company>(companyViewModel);

            await _companyService.PostCompany(company);

            await _companyService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates new employee entry
        /// 
        /// </summary>
        /// <remarks>
        /// Admin, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idCompany"></param>
        /// <param name="employeeWithDetailsViewModel"></param>
        /// <returns>Employee detail</returns>
        /// <response code="201">Created employee</response>
        [HttpPost("{idCompany}/Employee")]
        [Authorize(Policy = "Admin_Manager_Member")]
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
