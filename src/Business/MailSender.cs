using System.Net.Mail;
using EternalBAND.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EternalBAND.Business;

 public class MailSender : IEmailSender
    {
        private ApplicationDbContext _entities;
        public MailSender(ApplicationDbContext entities)
        {
            _entities = entities;
        }
        // public async Task SendEmailAddAttachmentsAsync(string email, string subject, string htmlMessage,string attachmentPath)
        // {
        //     try
        //     {
        //         var systemInfos =  new Business.Repository.BaseRepository<SystemInfo>(_entities).GetAll().Result.ToList();
        //
        //         var senderMail = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-address").Value;
        //         var senderMailPassword = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-address-password").Value;
        //         var senderDisplayName = systemInfos.FirstOrDefault(n => n.Type == "site-title").Value;
        //         if (senderMail != null && senderDisplayName != null && senderMailPassword != null)
        //         {
        //             MailMessage mail = new MailMessage();
        //             mail.IsBodyHtml = true;
        //             mail.To.Add(email);
        //             mail.From = new MailAddress(senderMail, senderDisplayName, System.Text.Encoding.UTF8);
        //             mail.Subject = subject;
        //             mail.Body = new Business.Classes.GeneratorMailTemplate(htmlMessage).GenerateEmail();
        //             SmtpClient smtpClient = new SmtpClient();
        //             smtpClient.Timeout = 10000;
        //             smtpClient.Credentials = new System.Net.NetworkCredential(senderMail, senderMailPassword);
        //             smtpClient.Port = int.Parse(systemInfos.FirstOrDefault(n => n.Type == "mail-sender-port").Value);
        //             smtpClient.Host = systemInfos.FirstOrDefault(n => n.Type == "mail-sender-host").Value;
        //             smtpClient.EnableSsl = bool.Parse(systemInfos.FirstOrDefault(n => n.Type == "mail-sender-enable-ssl").Value);
        //             System.Net.Mail.Attachment attachment;
        //             attachment = new System.Net.Mail.Attachment(attachmentPath);
        //             mail.Attachments.Add(attachment);
        //             await smtpClient.SendMailAsync(mail);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         await new LogService(_entities).ErrorLogMethod("Mail gönderilirken hata.", ex,"SendEmailAsync");
        //
        //     }
        // }
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
                    mail.Body = new Business.GeneratorMailTemplate(htmlMessage).GenerateEmail();
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Timeout = 10000;
                    smtpClient.Credentials = new System.Net.NetworkCredential(senderMail, senderMailPassword);
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