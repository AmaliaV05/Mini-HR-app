using Mini_HR_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string content);
    }
}
