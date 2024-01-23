using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EternalBAND.Api.Services
{
    public class MessageService
    {
        private ApplicationDbContext _context;
        private readonly IEmailSender _mailsender;
        private readonly HubController _hubController;
        public MessageService(ApplicationDbContext context, IEmailSender mailsender, HubController hubController)
        {
            _context = context;
            _mailsender = mailsender;
            _hubController = hubController;
        }


        public async Task SendAndBroadCastMessageAsync(Users currentUser, Guid receiverUserId, string? message, int postId)
        {
            var msg = await SaveMessage(currentUser, receiverUserId, message, postId);
            var postTitle = _context.Posts.Find(msg.RelatedPostId).Title;
            await _hubController.BroadcastMessage("ReceiveMessage", JsonSerializer.Serialize(msg));
            var notif = await CreateMessageNotification(
                $"{currentUser.Name} '{postTitle}' başıklı ilana mesaj gönderdi.",
                currentUser.Id,
                receiverUserId.ToString(),
                msg.RedirectLink,
                msg.Id.ToString());
            await _hubController.BroadcastMessage("ReceiveMessageNotification", JsonSerializer.Serialize(notif));
        }

        public IEnumerable<MessageBox> GetAllMessageBoxes(string userId)
        {
            var allMessages = _context.Messages.Include(n => n.ReceiverUser).Include(n => n.SenderUser).Include(p => p.RelatedPost)
                .Where(n => n.SenderUserId == userId || n.ReceiverUserId == userId);
            return GroupByMetadata(allMessages);
        }

        public async Task<MessageBox> GetOrCreteMessageBox(string receiverUserId, string senderUSerId, int postId)
        {
            var messageBox = GetMessageBox(receiverUserId, senderUSerId, postId);
            foreach (var msg in messageBox.Messages.Where(n => !n.IsRead).ToList())
            {
                msg.IsRead = true;

                var notifs = _context.Notification.Where(s =>
                    s.NotificationType == Common.NotificationType.Message &&
                    s.RelatedElementId == msg.Id.ToString() &&
                    !s.IsRead
                );
                foreach (var not in notifs)
                {
                    not.IsRead = true;
                }
                await _context.Commit();
            }
            return messageBox;
        }

        private IEnumerable<MessageBox> GroupByMetadata(IEnumerable<Messages> messages)
        {
            return messages
                .GroupBy(n => new { n.RelatedPostId, Recipients = string.Join(",", new[] { n.SenderUserId, n.ReceiverUserId }.OrderBy(s => s)) })
                .Select(s => new MessageBox(new MessageMetadata(s.Key.RelatedPostId, s.Key.Recipients.Split(",")), s.Select(s => s).ToList()));
        }

        private MessageBox? GetMessageBox(string receiverUserId, string senderUserId, int postId)
        {
            var allMessages = _context.Messages
                .Include(n => n.ReceiverUser)
                .Include(n => n.SenderUser)
                .Where(n =>
                    (n.ReceiverUserId == receiverUserId || n.ReceiverUserId == senderUserId)
                    && (n.SenderUserId == senderUserId || n.SenderUserId == receiverUserId)
                    && n.RelatedPostId == postId);
            return GroupByMetadata(allMessages).FirstOrDefault() ?? new MessageBox(new MessageMetadata(postId, new[] { senderUserId, receiverUserId }.OrderBy(s => s).ToArray()));
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

        private async Task<Notification> CreateMessageNotification(string message, string currentUserId, string receiverUserId, string redirectLink, string seo)
        {
            try
            {
                var notif = new Notification()
                {
                    IsRead = false,
                    AddedDate = DateTime.Now,
                    Message = message,
                    ReceiveUserId = receiverUserId,
                    SenderUserId = currentUserId,
                    RedirectLink = redirectLink,
                    // TODO : message's related id is just post now maybe better to give user + post on following time
                    RelatedElementId = seo,
                    NotificationType = Common.NotificationType.Message

                };
                await _context.Notification.AddAsync(notif);
                await _context.SaveChangesAsync();
                return notif;
            }
            catch (Exception ex)//TODO:log
            {
                throw;
            }
        }

        // TODO remove SMTP related function
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
}
