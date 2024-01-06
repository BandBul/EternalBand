using EternalBAND.Data;
using EternalBAND.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EternalBAND.Business;

public class MessageService
{
    private ApplicationDbContext _context;
    private readonly IEmailSender _mailsender;

    public MessageService(ApplicationDbContext context, IEmailSender mailsender)
    {
        _context = context;
        _mailsender = mailsender;
    }
    public async Task CreateMessageNotification(string message, string receiverUserId,string redirectLink, string seo)
    {
        try
        {
            await _context.Notification.AddAsync(new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                Message = message,
                ReceiveUserId = receiverUserId,
                RedirectLink = redirectLink,
                // TODO : message's related id is just post now maybe better to give user + post on following time
                RelatedElementId = seo,
                NotificationType = Common.NotificationType.Message

            });
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)//TODO:log
        {
            throw;
        }
    }
    public async Task<bool> SendMessageAsync(string message, string receiverUserId)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Id == receiverUserId);
            await _mailsender.SendEmailAsync(user.Email, "Yeni Bir Bildirimin Var", message);
            return true;
        }
        catch (Exception ex)//TODO:log
        {
            return false;
        }
    }

}