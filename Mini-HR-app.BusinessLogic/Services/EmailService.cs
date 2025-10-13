using Microsoft.Extensions.Configuration;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.Exceptions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string content)
        {
            string apiKey = _configuration["SendGrid:SENDGRID_API_DEVELOP_KEY"];
            SendGridClient client = new SendGridClient(apiKey);
            string senderEmail = _configuration["SendGrid:Sender_Email_Address"];
            string senderName = _configuration["SendGrid:Sender_Name"];
            EmailAddress sender = new EmailAddress(senderEmail, senderName);           
            EmailAddress recipient = new EmailAddress(toEmail);
            SendGridMessage message = MailHelper.CreateSingleEmail(sender, recipient, subject, content, content);
            Response response = await client.SendEmailAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new SendGridException($"Failed to send email to {toEmail}. Status Code: {response.StatusCode}");
            }    
        }
    }
}
