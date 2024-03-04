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
        public MessageService(ApplicationDbContext context, BroadCastingManager broadCastingManager)
        {
            _context = context;
            _broadcastingManager = broadCastingManager;
        }

        public async Task SendAndBroadCastMessageAsync(Users currentUser, Guid receiverUserId, string? message, int postId, int messageBoxId)
        {
            var msg = await _broadcastingManager.CreateMessage(currentUser, receiverUserId.ToString(), postId, message, messageBoxId);
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
            return _context.MessageBoxes.Where(MessageBox.IsRecipientPredicate(userId));
        }

        public async Task<MessageBox> GetOrCreateMessageBox(string receiverUserId, string senderUSerId, int postId)
        {
            var messageBox = GetMessageBox(receiverUserId, senderUSerId, postId);
            if(messageBox == null)
            {
                messageBox = new MessageBox()
                {
                    Recipient1 = receiverUserId,
                    Recipient2 = senderUSerId,
                    PostId = postId,
                    PostIdBackup = postId,
                    PostTitle = _context.Posts.Find(postId).Title,
                    IsPostDeleted = false
                };
                _context.Add(messageBox);
                await _context.Commit();
            }
            else
            {
                if(messageBox.Messages != null)
                {
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
                }
            }
            return _context.MessageBoxes.Include(s => s.Messages).FirstOrDefault(s => s.Id == messageBox.Id);
        }

        private MessageBox? GetMessageBox(string receiverUserId, string senderUserId, int postId)
        {
            var isPostExist = IsPostsExists(postId);
            return _context.MessageBoxes.FirstOrDefault(MessageBox.IsMessageBoxExistPredicate(receiverUserId, senderUserId, postId, isPostExist));
        }

        public bool IsPostsExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
