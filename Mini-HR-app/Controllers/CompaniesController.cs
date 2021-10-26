using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.Exceptions;
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

            if (companies == null)
            {
                return BadRequest("Max year of establishment cannot be less than min year of establishment");
            }

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

            if (company == null)
            {
                return NotFound("The company does not exist");
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
        /*[Authorize(Roles = "Manager")]*/
        public async Task<ActionResult<CompanyWithEmployeesViewModel>> GetActiveEmployees(int idCompany)
        {
            var company = await _companyService.GetActiveEmployees(idCompany);

            if (company == null)
            {
                return NotFound("The company does not exist");
            }

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
            var checkCompany = await _companyService.CheckCompanyIsActive(idCompany);

            if (!checkCompany)
            {
                return BadRequest("Company does not exist");
            }

            var checkEmployee = await _companyService.CheckEmployeeIsActive(idEmployee);

            if (!checkEmployee)
            {
                return BadRequest("Employee does not exist");
            }

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
            var company = _mapper.Map<Company>(companyViewModel);

            try
            {
                company = _mapper.Map<Company>(companyViewModel);
            }
            catch (InvalidCompanyException icEx)
            {
                Console.WriteLine(icEx.Message);
            }

            if (company.Id != idCompany)
            {
                return BadRequest("Company id does not match the input id");
            }

            var companyEditSuccesful = await _companyService.PutCompanyDetails(idCompany, company);

            if (!companyEditSuccesful)
            {
                return NotFound();
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
            var employee = _mapper.Map<Employee>(employeeWithDetailsViewModel);

            if (idEmployee != employee.Id)
            {
                return BadRequest();
            }

            var employeeEditSuccesfull = await _companyService.PutEmployeeDetails(idCompany, idEmployee, employee);

            if (!employeeEditSuccesfull)
            {
                return NotFound();
            }

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
            var checkCompany = await _companyService.CheckCompanyIsActive(idCompany);

            if (!checkCompany)
            {
                return BadRequest("Company does not exist");
            }

            var companyEditSuccesful = await _companyService.PutCompanyStatusToInactive(idCompany);

            if (!companyEditSuccesful)
            {
                return BadRequest("The company still has employees");
            }

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
            var checkCompany = await _companyService.CheckCompanyIsActive(idCompany);

            if (!checkCompany)
            {
                return BadRequest("Company does not exist");
            }

            var checkEmployee = await _companyService.CheckEmployeeIsActive(idEmployee);

            if (!checkEmployee)
            {
                return BadRequest("Employee does not exist");
            }

            var company = await _companyService.PutEmployeeStatusToInactive(idCompany, idEmployee);

            if (!company)
            {
                return BadRequest("The employee is not active at the chosen company");
            }

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

            var checkCompany = await _companyService.PostCompany(company);

            if (!checkCompany)
            {
                return BadRequest("Company already exists");
            }

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

            var checkCompany = await _companyService.PostEmployeeForCompany(idCompany, employee);

            if (!checkCompany)
            {
                return NotFound("Company does not exist");
            }

            return NoContent();
        }
    }
}
