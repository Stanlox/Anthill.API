using Anthill.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Anthill.API.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> logger;
        private Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public void SendEmail(string recipient, string subject, string textMessage)
        {
            try
            {
                MailMessage message = new MailMessage();
                if (tagRegex.IsMatch(textMessage))
                {
                    message.IsBodyHtml = true;
                }
                else
                {
                    message.IsBodyHtml = false;
                }
                message.From = new MailAddress("maxim.bondaruk2000@gmail.com", "Athill.by");
                message.To.Add(recipient);
                message.Subject = subject;
                message.Body = textMessage;

                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Credentials = new NetworkCredential("maxim.bondaruk2000@gmail.com", "37529913603434Vazubu22062000");
                    client.Port = 587;
                    client.EnableSsl = true;

                    client.Send(message);
                    logger.LogInformation("Сообщение успешно отправлено!");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetBaseException().Message);
            }
        }
    }
}
