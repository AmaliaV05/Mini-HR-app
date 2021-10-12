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
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of employees with personal details
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWithDetailsViewModel>>> GetEmployee()
        {
            return await _context.Employee
                .Include(e => e.Person)
                .Select(e => _mapper.Map<EmployeeWithDetailsViewModel>(e))
                .ToListAsync();
        }   
    }
}
