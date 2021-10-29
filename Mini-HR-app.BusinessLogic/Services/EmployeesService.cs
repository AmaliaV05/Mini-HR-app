using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.Data;
using Mini_HR_app.Data.Models;
using Mini_HR_app.Exceptions;
using Mini_HR_app.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class EmployeesService : IEmployeesService
    {
        public readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public EmployeesService(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task AddPhoto(int idEmployee, IFormFile file)
        {
            var employee = await _context.Employees.FindAsync(idEmployee);

            if (employee.Photo != null)
            {
                throw new GetEmployeeException($"Employee with id: {idEmployee} does not have photo");
            }

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null)
            {
                throw new CloudinaryException("Cannot add photo");
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            employee.Photo = photo;
        }

        public async Task<Employee> GetEmployeePhoto(int idEmployee)
        {
            var employee =  await _context.Employees
                .Where(e => e.Id == idEmployee)
                .Include(e => e.Photo)                
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new GetEmployeeException($"Employee with id: {idEmployee} does not exist");
            }

            return employee;
        }

        public async Task DeletePhoto(int idEmployee)
        {
            var employee = await _context.Employees
                .Where(e => e.Id == idEmployee)
                .FirstAsync();

            if (employee == null)
            {
                throw new GetEmployeeException($"Employee with id: {idEmployee} does not exist");
            }

            var photo = employee.Photo;

            if (photo == null)
            {
                throw new GetEmployeeException($"Employee with id: {idEmployee} does not have photo");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhoto(photo.PublicId);
                if (result.Error != null)
                {
                    throw new CloudinaryException("Cannot delete photo");
                }
            }

            _context.Photos.Remove(photo);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
