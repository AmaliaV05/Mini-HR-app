using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Mini_HR_app.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Mini_HR_app.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Mini_HR_app.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SENDGRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mazesoftware@hotmail.com", "Demo email sending");           
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
