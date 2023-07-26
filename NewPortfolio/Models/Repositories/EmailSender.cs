using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace NewPortfolio.Models.Repositories
{
    public class EmailSender : IEmailSender
    {


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = "idontcryadmin@seznam.cz";
            var pw = "upes123";

            var client = new SmtpClient("smtp.seznam.cz", 465)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,pw)
            };

            return client.SendMailAsync(new MailMessage(from: mail,
                to: email, subject, htmlMessage));
        }

        public void SendEmail()
        {

        }
    }
}
