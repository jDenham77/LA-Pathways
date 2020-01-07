using Microsoft.AspNetCore.Http;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class EmailService : IEmailService
    {
        IDataProvider _data = null;
        public EmailService(IDataProvider data)
        {
            _data = data;
        }
        private async Task<Response> Send(SendGridMessage message, SendGridClientOptions apiKey)
        {
            var client = new SendGridClient(apiKey);
            return await client.SendEmailAsync(message);
        }
        public async Task<Response> ConfirmEmail(string email, Guid token, SendGridClientOptions apiKey)
        {
            string confirmSubject = "Please confirm your account";
            var model = new EmailAddRequest();
            model.To = email;
            model.From = "admin@lapathways.org";
            model.Subject = confirmSubject;

            string directory = Environment.CurrentDirectory;
            string path = Path.Combine(directory, "EmailTemplates\\EmailConfirm.html");
            string _htmlContent = System.IO.File.ReadAllText(path);
            string content = _htmlContent.Replace("{{confirmLink}}", "https://lapathway.azurewebsites.net/confirm/" + token);

            SendGridMessage message = new SendGridMessage()
            {
                From = new EmailAddress(model.From),
                Subject = model.Subject,
                HtmlContent = content
            };
            message.AddTo(model.To);
            return await Send(message, apiKey);
        }
        public async Task<Response> SendEmails(EmailListAddRequest model, SendGridClientOptions apiKey)
        {
            List<EmailAddress> list = null;
            string directory = Environment.CurrentDirectory;
            string path = Path.Combine(directory, "EmailTemplates\\ResourceEmail.html");
            string _htmlContent = System.IO.File.ReadAllText(path);
            string content = _htmlContent.Replace("{{confirmLink}}", "https://lapathway.azurewebsites.net/api/resources/emails/");

            foreach (var em in model.Emails)
            {
                EmailAddress _email = new EmailAddress(em);
                if (list == null)
                {
                    list = new List<EmailAddress>();
                }
                list.Add(_email);

            }

            SendGridMessage message = new SendGridMessage()
            {
                From = new EmailAddress("admin@lapathways.org"),
                Subject = "Update info",
                HtmlContent = content
            };
            message.AddTo("bathman2@mailinator.com");
            message.AddBccs(list);

            return await Send(message, apiKey);
        }
        public async Task<Response> ResetEmail(string email, Guid token, SendGridClientOptions apiKey)
        {
            SendGridMessage message = new SendGridMessage();
            string directory = Environment.CurrentDirectory;
            string path = Path.Combine(directory, "EmailTemplates\\ResetPassword.html");
            string _htmlContent = System.IO.File.ReadAllText(path);
            string htmlContent = _htmlContent.Replace("{{resetLink}}", "https://lapathway.azurewebsites.net/reset/" + token);
            string plainTextContent = "Reset Password";
            var from = new EmailAddress("admin@lapathways.org", "LA Pathways Admin");
            var subject = "Please Reset Password";
            var to = new EmailAddress(email, "User");
            message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return await Send(message, apiKey);
        }
        public async Task<Response> ContactEmail(ContactAddRequest model, SendGridClientOptions apiKey)
        {
            string directory = Environment.CurrentDirectory;
            string path = Path.Combine(directory, "EmailTemplates\\Contact.html");
            string _htmlContent = System.IO.File.ReadAllText(path);
            string contents = _htmlContent.Replace("{{fName}}", model.FirstName)
                                         .Replace("{{lName}}", model.LastName)
                                         .Replace("{{email}}", model.email)
                                         .Replace("{{contentss}}", model.message);

            SendGridMessage message = new SendGridMessage()
            {
                From = new EmailAddress(model.email),
                Subject = "Contact Info",
                HtmlContent = contents
            };
            message.AddTo("admin@lapathways.org");
            return await Send(message, apiKey);
        }

        public async Task<Response> EmailResourcePdf(IFormFile file, string email, SendGridClientOptions apiKey)
        {
            string directory = Environment.CurrentDirectory;
            string path = Path.Combine(directory, "EmailTemplates\\EmailResourcesPdf.html");
            string _htmlContent = System.IO.File.ReadAllText(path);
            SendGridMessage message = new SendGridMessage()
            {
                From = new EmailAddress("admin@lapathways.org"),
                Subject = "Your resource recommendations.",
                HtmlContent = _htmlContent

            };

            var memoryStream = new MemoryStream();

            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(fileBytes);
            message.AddTo(email);
            message.AddAttachment("Resources.pdf", base64String, "application/pdf");

            return await Send(message, apiKey);
        }
    }
}