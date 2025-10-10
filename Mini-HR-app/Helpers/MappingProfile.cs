using AutoMapper;
using Mini_HR_app.Data.Models;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels;

namespace Mini_HR_app
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyViewModel>().ReverseMap();
            CreateMap<Company, CompanyWithEmployeesViewModel>();
            CreateMap<Employee, EmployeeWithDetailsViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeWithPhotoViewModel>();
            CreateMap<Photo, PhotoViewModel>().ReverseMap();
        }
    }
}
