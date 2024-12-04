using System.Net;
using System.Net.Mail;
using EternalBAND.Api.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EternalBAND.Api;

public class EmailSender : IBaseEmailSender
{
    private readonly SmtpOptions smtpSettings;
    private readonly SiteGeneralOptions siteSettings;
    private readonly ILogger<EmailSender> logger;
    private SmtpClient smtpClient;
    public EmailSender(
        IOptions<SmtpOptions> smtpOptions, 
        IOptions<SiteGeneralOptions> siteOptions, 
        ILogger<EmailSender> logger)
    {
        smtpSettings = smtpOptions.Value;
        siteSettings = siteOptions.Value;
        this.logger = logger;
        SetSmtpClient();
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var senderMail = smtpSettings.SenderAddress;
            var senderDisplayName = siteSettings.Name;
            var emailGenerator =
                new EmailGenerator()
                .SetContent(htmlMessage)
                .SetEmailTitle(senderDisplayName)
                .SetTitle(senderDisplayName);

            MailMessage mail = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = emailGenerator.GenerateEmail()
            };
            mail.To.Add(email);
            mail.From = new MailAddress(senderMail, senderDisplayName, System.Text.Encoding.UTF8);

            await smtpClient.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error happened during email sent");
        }
    }

    private void SetSmtpClient()
    {
        if (smtpClient == null)
        {
            smtpClient = new SmtpClient()
            {
                Timeout = smtpSettings.TimeoutMs,
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
                Port = smtpSettings.Port,
                Host = smtpSettings.Host,
                EnableSsl = smtpSettings.SslEnabled
            };
        }
    }
}