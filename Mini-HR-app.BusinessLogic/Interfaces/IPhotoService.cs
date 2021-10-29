using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<DeletionResult> DeletePhoto(string publicId);
    }
}
