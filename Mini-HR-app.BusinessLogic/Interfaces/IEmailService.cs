using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string content);
    }
}
