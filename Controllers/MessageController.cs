using EternalBAND.Business;
using EternalBAND.Business.Options;
using EternalBAND.Data;
using EternalBAND.Hubs;
using EternalBAND.Models;
using EternalBAND.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace EternalBAND.Controllers;

public class MessageController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<Users> _userManager;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly MessageService _messageService;


    public MessageController(ApplicationDbContext context, UserManager<Users> userManager, IHubContext<ChatHub> hubContext, MessageService notificationProcess)
    {
        _context = context;
        _userManager = userManager;
        _hubContext = hubContext;
        _messageService = notificationProcess;
    }

    private IEnumerable<MessageBox> GetAllMessageBoxes(string userId)
    {
        var allMessages = _context.Messages.Include(n => n.ReceiverUser).Include(n => n.SenderUser).Include(p => p.RelatedPost)
            .Where(n => n.SenderUserId == userId || n.ReceiverUserId == userId);
        return GroupByMetadata(allMessages);
    }

    private IEnumerable<MessageBox> GroupByMetadata(IEnumerable<Messages> messages)
    {
        return messages
            .GroupBy(n => new { n.RelatedPostId, Recipients= string.Join(",", new[] { n.SenderUserId, n.ReceiverUserId }.OrderBy(s => s)) })
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

    [Route("mesajlar/{userId?}")]
    // userId : message receiver userId
    // postId : mssage is being sent for related postId
    public async Task<ActionResult> ChatIndex(string? userId, int postId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var chats = GetAllMessageBoxes(user.Id).ToList();
            ChatViewModel viewModel = new() { AllChat = chats };
            ViewBag.LoginUserId = user.Id;

            if (userId != null)
            {
                ViewBag.ReceiverUserId = userId;
                ViewBag.PostId = postId;
                var messageBox = GetMessageBox(userId, user.Id, postId);
                foreach (var isread in messageBox.Messages.Where(n => !n.IsRead).ToList())
                {
                    isread.IsRead = true;

                    var notifs = _context.Notification.Where(s =>
                        s.NotificationType == Common.NotificationType.Message &&
                        s.RelatedElementId == isread.Id.ToString() &&
                        !s.IsRead
                    );
                    foreach(var not in notifs)
                    {
                        not.IsRead = true;
                    }
                    await _context.SaveChangesAsync();
                }
                
                viewModel.CurrentChat = messageBox;

            }
            // coming from My messages
            return View(viewModel);
        }
        else
        {
            return Redirect("/giris-yap");
        }
    }
    // TODO : add logging before each return
    [HttpPost, ActionName("SendMessage")]
    public async Task<ActionResult> SendMessage(Guid id, string message, int postId)
    {
        if (!_context.Users.Any(n => n.Id == id.ToString()))
        {
            return Json("Kay�t bulunamad�.");
        }
        var getUser = await _userManager.GetUserAsync(User);
        if (id.ToString().Equals(getUser.Id))
        {
            return Json("User can not send message to yourself");
        }
        try
        {
            await _messageService.SendMessageAsync(getUser, id, message, postId);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

}