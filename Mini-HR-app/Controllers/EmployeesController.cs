using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.Data.Models;
using Mini_HR_app.ViewModels;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin_Manager_Member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly IMapper _mapper;

        public EmployeesController(IMapper mapper, IEmployeesService employeesService)
        {
            _mapper = mapper;
            _employeesService = employeesService;
        }

        /// <summary>
        /// Add a new photo to an employee
        /// </summary>
        /// <remarks>
        /// Admins, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idEmployee"></param>
        /// <param name="file"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPost("{idEmployee}/Add-Photo")]
        public async Task<ActionResult> AddPhoto(int idEmployee, IFormFile file)
        {
            await _employeesService.AddPhoto(idEmployee, file);

            await _employeesService.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Get photo attached to an employee
        /// </summary>
        /// <remarks>
        /// Admins, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idEmployee"></param>
        /// <returns>Employee with profile photo</returns>
        /// <response code="200">Employee with profile photo successfully retrieved</response>
        [HttpGet("{idEmployee}/Photo")]
        public async Task<ActionResult<EmployeeWithPhotoViewModel>> GetEmployeePhoto(int idEmployee)
        {
            var employee = await _employeesService.GetEmployeePhoto(idEmployee);

            return _mapper.Map<EmployeeWithPhotoViewModel>(employee);
        }

        /// <summary>
        /// Deletes employee photo
        /// </summary>
        /// <remarks>
        /// Admins, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idEmployee"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpDelete("{idEmployee}/Delete-Photo")]
        public async Task<ActionResult> DeletePhoto(int idEmployee)
        {
            await _employeesService.DeletePhoto(idEmployee);

            await _employeesService.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Allows to add contact information for employees
        /// </summary>
        /// <remarks>
        /// Admins, Managers and Members can access this endpoint
        /// </remarks>
        /// <param name="idEmployee"></param>
        /// <param name="contact"></param>
        /// <returns>Empty response</returns>
        /// <response code="204">No content</response>
        [HttpPost("{idEmployee}/Contact-Details")]
        public async Task<ActionResult> PostEmployeeContact(int idEmployee, Contact contact)
        {
            await _employeesService.PostEmployeeContact(idEmployee, contact);
            
            return NoContent();
        }
    }
}