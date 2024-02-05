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
        private readonly BroadCastingManager _broadcastingManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _mailsender;
        public MessageService(ApplicationDbContext context, IEmailSender mailsender, BroadCastingManager broadCastingManager)
        {
            _context = context;
            _mailsender = mailsender;
            _broadcastingManager = broadCastingManager;
        }

        public async Task SendAndBroadCastMessageAsync(Users currentUser, Guid receiverUserId, string? message, int postId)
        {
            var msg = await _broadcastingManager.CreateMessage(currentUser, receiverUserId.ToString(), postId, message);
            var post = _context.Posts.Find(msg.RelatedPostId);
            if(post == null || post.Status != Common.PostStatus.Active ) 
            {
                throw new JsonException("Yayında olmayan yayına mesaj gönderemezsiniz.");
            }
            var postTitle = post.Title;
            await _broadcastingManager.CreateMessageNotification(
                $"{currentUser.Name} '{postTitle}' başlıklı ilana mesaj gönderdi.",
                msg);
        }

        public IEnumerable<MessageBox> GetAllMessageBoxes(string userId)
        {
            var allMessages = _context.Messages.Include(n => n.ReceiverUser).Include(n => n.SenderUser)
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
