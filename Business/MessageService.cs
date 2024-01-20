using EternalBAND.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using EternalBAND.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using EternalBAND.DomainObjects;

namespace EternalBAND.Business;

public class MessageService
{
    private ApplicationDbContext _context;
    private readonly IEmailSender _mailsender;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService(ApplicationDbContext context, IEmailSender mailsender, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _mailsender = mailsender;
        _hubContext = hubContext;
    }


    public async Task SendAndBroadCastMessageAsync(Users currentUser, Guid receiverUserId, string? message, int postId)
    {
        var msg = await SaveMessage(currentUser, receiverUserId, message, postId);
        var postTitle = _context.Posts.Find(msg.RelatedPostId).Title;
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", JsonSerializer.Serialize(msg));
        var notif = await CreateMessageNotification(
            $"{currentUser.Name} '{postTitle}' baþýklý ilana mesaj gönderdi.",
            currentUser.Id,
            receiverUserId.ToString(),
            msg.RedirectLink,
            msg.Id.ToString());
        await _hubContext.Clients.All.SendAsync("ReceiveMessageNotification", JsonSerializer.Serialize(notif));
    }


    private async Task<Messages> SaveMessage(Users currentUser, Guid receiverUserId, string? message, int postId)
    {
        var messages = new Messages()
        {
            SenderUserId = currentUser.Id,
            IsRead = false,
            Date = DateTime.Now,
            Message = message,
            MessageGuid = Guid.NewGuid(),
            ReceiverUserId = receiverUserId.ToString(),
            RelatedPostId = postId
        };
        await _context.Messages.AddAsync(messages);
        messages.SenderUser = currentUser;
        await _context.SaveChangesAsync();
        return messages;
    }

    private async Task<Notification> CreateMessageNotification(string message, string currentUserId, string receiverUserId,string redirectLink, string seo)
    {
        try
        {
            var notif = new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                Message = message,
                ReceiveUserId = receiverUserId,
                SenderUserId =currentUserId,
                RedirectLink = redirectLink,
                // TODO : message's related id is just post now maybe better to give user + post on following time
                RelatedElementId = seo,
                NotificationType = Common.NotificationType.Message

            };
            await _context.Notification.AddAsync(notif);
            await _context.SaveChangesAsync();
            return notif;
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