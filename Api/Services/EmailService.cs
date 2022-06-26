using System.Net.Mail;
using System.Net;

namespace Api.Services
{
    public interface IEmailService
    {
        void Send(string to);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to)
        {
            string host = _configuration.GetValue<string>("EmailSettings:SmtpHost");
            string user = _configuration.GetValue<string>("EmailSettings:SmtpUser");
            string password = _configuration.GetValue<string>("EmailSettings:SmtpPassword");
            int port = _configuration.GetValue<int>("EmailSettings:SmtpPort");

            string subject = "Trocar a senha";

            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(user, "Teste")
            };

            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = "Teste";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using (SmtpClient smtp = new SmtpClient(host, port))
            {
                smtp.Credentials = new NetworkCredential(user, password);
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
        }
    }
}

