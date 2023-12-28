using EternalBAND.Data;
using EternalBAND.Models;
using EternalBAND.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System;

namespace EternalBAND.Controllers;

public class MessageController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<Users> _userManager;

    public MessageController(ApplicationDbContext context, UserManager<Users> userManager)
    {
        _context = context;
        _userManager = userManager;
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
                var chat = GetMessageBox(userId, user.Id, postId);
                // TO DO implement isRead
                //foreach (var isread in chat.Where(n => !n.IsRead).ToList())
                //{
                //    isread.IsRead = true;
                //    await _context.SaveChangesAsync();
                //}
                viewModel.CurrentChat = chat;
                
            }
            // coming from My messages
            return View(viewModel);
        }
        else
        {
            return Redirect("/giris-yap");
        }
    }

    [HttpPost, ActionName("SendMessage")]
    public async Task<JsonResult> SendMessage(Guid id, string message, int postId)
    {
        var seo = _context.Posts.Where(s => s.Id == postId).Select(s => s.SeoLink).FirstOrDefault();
        if (!_context.Users.Any(n => n.Id == id.ToString()))
        {
            return Json("Kayýt bulunamadý.");
        }

        var getUser = await _userManager.GetUserAsync(User);
        if (id.ToString().Equals(getUser.Id))
        {
            return Json("User can not send message to yourself");
        }
        try
        {
            await _context.Messages.AddAsync(new Messages()
            {
                SenderUserId = getUser.Id,
                IsRead = false,
                Date = DateTime.Now,
                Message = message,
                MessageGuid = Guid.NewGuid(),
                ReceiverUserId = id.ToString(),
                RelatedPostId = postId
            });
            // TODO embed this to IOC level
            await new Business.NotificationProcess(_context).SaveNotification($"{getUser.Name} sana bir mesaj gönderdi.", id.ToString(),
                 $"mesajlar/{getUser.Id}?postId={postId}" , seo);

            await _context.SaveChangesAsync();
            return Json("Mesaj gönderildi.");
        }
        catch (Exception ex)
        {
            return Json("Mesaj gönderilemedi.");
        }
    }

}