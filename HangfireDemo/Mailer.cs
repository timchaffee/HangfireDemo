using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HangfireDemo
{
    public class Mailer
    {
        private static async Task SendmailAsync(string from, string to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "127.0.0.1", // set your SMTP server name here
                Port = 25, // Port 
                EnableSsl = false,
                Credentials = new NetworkCredential(from, "password")
            };

            using (MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body + Environment.NewLine + Environment.NewLine + $"Sent at {DateTime.Now}"
            })
            {
                await smtpClient.SendMailAsync(message);
            }
        }
        public async Task SendHelloMailAsync(string from, string to, string subject, string body)
        {
            await Task.Delay(30000);
            await Mailer.SendmailAsync(from, to, subject,body);
        }
    }


}
