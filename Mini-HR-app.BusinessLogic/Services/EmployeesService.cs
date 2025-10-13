using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.BusinessLogic.Models;
using Mini_HR_app.Data;
using Mini_HR_app.Data.Models;
using Mini_HR_app.Data.Models.Constants.Utils;
using Mini_HR_app.Exceptions;
using Mini_HR_app.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IEmailService _emailService;
        private readonly AuditTrailService _auditTrailService;
        private readonly ICurrentUserService _currentUserService;

        public EmployeesService(ApplicationDbContext context, IPhotoService photoService, IEmailService emailService, ICurrentUserService currentUserService)
        {
            _context = context;
            _photoService = photoService;
            _emailService = emailService;
            _currentUserService = currentUserService;
            _auditTrailService = AuditTrailService.Instance(new AuditTrailFactory());
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

            CreateAuditLog(AuditAction.Create, nameof(Photo), photo.Id);

            // FIXME: wrap in a database transaction to avoid saving audit log if creation fails
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
                .Include(e => e.Photo)
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

            CreateAuditLog(AuditAction.Delete, nameof(Photo), photo.Id);

            // FIXME: wrap in a database transaction to avoid saving audit log if deletion fails
        }

        public async Task PostEmployeeContact(int idEmployee, Contact contact)
        {
            var employee = await _context.Employees
                .Where(e => e.Id == idEmployee)
                .Include(e => e.Contacts)
                .FirstOrDefaultAsync();

            if (contact == null)
            {
                throw new PostEmployeeException($"Contact details for employee with id: {idEmployee} cannot be empty");
            }

            employee.Contacts.Add(contact);
            await SaveChangesAsync();

            await _emailService.SendEmail(contact.Email, "Welcome", "<h2>" + employee.Name + " " + employee.Surname
                + ", you are now officially part of our group at Maze!</h2>");

            CreateAuditLog(AuditAction.Create, nameof(Contact), contact.Id);

            // FIXME: wrap in a database transaction to avoid saving audit log if creation fails
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private void CreateAuditLog(string action, string entityType, int entityId)
        {
            CurrentUser currentUser = _currentUserService.GetCurrentUser();

            AuditLog auditLog = _auditTrailService.LogChange(currentUser.Id, currentUser.MainRole, action, entityType, entityId);

            _context.AuditLogs.Add(auditLog);
        }
    }
}
