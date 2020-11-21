using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Repository
{
    public class SmtpEmailService : IEmailSender
    {
        private readonly ILogger<SmtpEmailService> _logger;
        public static IConfiguration Configuration { get; set; }

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    To = { email},
                    Body = htmlMessage,
                    From = new MailAddress("storesupport@gmail.com"),
                    Subject = subject
                };
                await GetClient().SendMailAsync(mailMessage);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
            }
        }
        
        public static SmtpClient GetClient()
        {
            var email = Configuration.GetValue<string>("SmtpCredentials:email");
            var password = Configuration.GetValue<string>("SmtpCredentials:password");

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password),
            };
            return client;
        }
    }
}