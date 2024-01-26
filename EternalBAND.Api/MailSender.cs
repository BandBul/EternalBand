using System.Net.Mail;
using EternalBAND.Api.Services;
using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EternalBAND.Api;

 public class MailSender : IEmailSender
    {
        private ApplicationDbContext _entities;
        public MailSender(ApplicationDbContext entities)
        {
            _entities = entities;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
               var systemInfos = _entities.SystemInfo.ToList();
        
                var senderMail = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-address").Value;
                var senderMailPassword = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-address-password").Value;
                var senderDisplayName = systemInfos.FirstOrDefault(n => n.Type == "site-title").Value;
                if (senderMail != null && senderDisplayName != null && senderMailPassword != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.To.Add(email);
                    mail.From = new MailAddress(senderMail, senderDisplayName, System.Text.Encoding.UTF8);
                    mail.Subject = subject;
                    mail.Body = new GeneratorMailTemplate(htmlMessage).GenerateEmail();
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Timeout = 10000;
                    smtpClient.Credentials = new System.Net.NetworkCredential(senderMail, senderMailPassword);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Port = int.Parse(systemInfos.FirstOrDefault(n => n.Type == "mail-sender-port").Value);
                    smtpClient.Host = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-host").Value;
                    smtpClient.EnableSsl = bool.Parse(systemInfos.FirstOrDefault(n => n.Type == "mail-sender-enable-ssl").Value);
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                await new LogService(_entities).ErrorLogMethod("Mail gönderilirken hata.", ex,"SendEmailAsync");
        
            }
        }
    }