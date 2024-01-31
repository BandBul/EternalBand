using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace EternalBAND.Api
{
    public class BroadCastingManager
    {
        private readonly UserManager<Users> _userManager;
        private readonly HubController _hubController; 
        private readonly ApplicationDbContext _context;
        public BroadCastingManager(UserManager<Users> userManager, HubController hubController, ApplicationDbContext context)
        {
            _userManager = userManager;
            _hubController = hubController;
            _context = context;
        }

        public async Task CreateUserNotification(Users currentUser, Posts post, string message)
        {
            // Engin-TODO need to send all admins
            // Engin-TODO pass SignalR hub and use ReceiveNotification broadcasting title to send message to front end
            var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
            var notif =  new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                NotificationType = NotificationType.PostSharing,
                ReceiveUserId = currentUser.Id,
                Message = message,
                SenderUserId = adminUsers.ElementAt(0).Id,
                RedirectLink = $"ilan/{post.SeoLink}",
                RelatedElementId = post.SeoLink
            };

            await SaveAndBroadCastNotification(notif, BroadCastingTitle.ReceiveNotification);
        }

        public async Task CreateCustomNotification(string senderUserId, string receiverUserId, Posts post, string message)
        {
            var notif = new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                NotificationType = NotificationType.PostSharing,
                ReceiveUserId = receiverUserId,
                Message = message,
                SenderUserId = senderUserId,
                RedirectLink = $"ilan/{post.SeoLink}",
                RelatedElementId = post.SeoLink
            };

            await SaveAndBroadCastNotification(notif, BroadCastingTitle.ReceiveNotification);
        }

        public async Task CreateAdminNotification(Users currentUser, Posts post, string message)
        {
            // Engin-TODO need to send all admins
            // Engin-TODO pass SignalR hub and use ReceiveNotification broadcasting title to send message to front end
            var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
            var notif = new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                NotificationType = NotificationType.PostSharing,
                ReceiveUserId = adminUsers.ElementAt(0).Id,
                Message = message,
                SenderUserId = currentUser.Id,
                RedirectLink = $"ilan/{post.SeoLink}?approvalPurpose=true",
                RelatedElementId = post.SeoLink
            };
            await SaveAndBroadCastNotification(notif, BroadCastingTitle.ReceiveNotification);
        }

        public async Task CreateMessageNotification(
            string notificationmessage,
            Messages msg)
        {
            var notif = new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                Message = notificationmessage,
                ReceiveUserId = msg.ReceiverUserId,
                SenderUserId = msg.SenderUserId,
                RedirectLink = msg.RedirectLink,
                // TODO : message's related id is just post now maybe better to give user + post on following time
                RelatedElementId = msg.Id.ToString(),
                NotificationType = NotificationType.Message

            };
            await SaveAndBroadCastNotification(notif, BroadCastingTitle.ReceiveMessageNotification);
        }

        public async Task<Messages> CreateMessage(Users currentUser, string receiverUserId, int postId, string message)
        {
            var msg = new Messages()
            {
                SenderUserId = currentUser.Id,
                IsRead = false,
                Date = DateTime.Now,
                Message = message,
                MessageGuid = Guid.NewGuid(),
                ReceiverUserId = receiverUserId,
                RelatedPostId = postId
            };

            await _context.Messages.AddAsync(msg);
            msg.SenderUser = currentUser;
            await _context.SaveChangesAsync();
            await _hubController.BroadcastMessage(BroadCastingTitle.ReceiveMessage, JsonSerializer.Serialize(msg));
            return msg;
        }

        private async Task SaveAndBroadCastNotification(Notification notif, string broadCastingTitle)
        {
            await _context.Notification.AddAsync(notif);
            await _context.SaveChangesAsync();

            await _hubController.BroadcastMessage(broadCastingTitle, JsonSerializer.Serialize(notif));
        }


    }
}
