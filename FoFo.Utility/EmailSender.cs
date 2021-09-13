using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FoFoStore.Utility
{
    public class EmailSender : IEmailSender //implement to solve the error with emailsender in the register
    {
        private readonly EmailOption emailOption;
        public EmailSender(IOptions<EmailOption> options)
        {
            emailOption = options.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(emailOption.SendGridKey,subject,htmlMessage,email);
        }
        private  Task Execute(string sendGridKey,string subject,string message,string email)
        {
            //من الجوجل ديفيلوبر 
            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("admin@FOFO.com", " FOFO Book");
            
            var to = new EmailAddress(email, "End User");
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            return client.SendEmailAsync(msg);
        }
    }
}
