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

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.cz");

                mail.From = new MailAddress("student.hanak@gmail.com");
                mail.To.Add("hanak.petr.net@eznam.cz");
                mail.Subject = "Test";
                mail.Body = "Ahoj, toto je zpráva poslána přes C# Aplikaci";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("student.hanak@gmail.com", "02087378Pe");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            
           
            catch (Exception)
            {

                throw;
            }
        }
    }
}
