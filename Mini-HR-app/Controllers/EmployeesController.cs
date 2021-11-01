using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.Data.Models;
using Mini_HR_app.ViewModels;

namespace Mini_HR_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        /// <param name="idEmployee"></param>
        /// <param name="file"></param>
        /// <returns>Photo</returns>
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
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpGet("{idEmployee}/Photo")]
        public async Task<ActionResult<EmployeeWithPhotoViewModel>> GetEmployeePhoto(int idEmployee)
        {
            var employee = await _employeesService.GetEmployeePhoto(idEmployee);

            return _mapper.Map<EmployeeWithPhotoViewModel>(employee);
        }

        /// <summary>
        /// Deletes employee photo
        /// </summary>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        [HttpDelete("{idEmployee}/Delete-Photo")]
        public async Task<ActionResult> DeletePhoto(int idEmployee)
        {
            await _employeesService.DeletePhoto(idEmployee);

            await _employeesService.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Contact-Details")]
        public async Task<ActionResult> PostEmployeeContact(int idEmployee, Contact contact)
        {
            await _employeesService.PostEmployeeContact(idEmployee, contact);
            
            return NoContent();
        }
    }
}