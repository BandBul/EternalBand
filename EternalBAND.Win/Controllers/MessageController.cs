using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Api.Services;
using EternalBAND.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using EternalBAND.Common;

namespace EternalBAND.Controllers;
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class MessageController : Controller
{
    private readonly MessageService _messageService;
    private readonly ControllerHelper _controllerHelper;

    public MessageController(MessageService messageService, ControllerHelper controllerHelper)
    {
        _messageService = messageService;
        _controllerHelper = controllerHelper;
    }


    [HttpGet, Route(EndpointConstants.MessagesEndpoint)]
    // userId : message receiver userId
    // postId : mssage is being sent for related postId
    public async Task<ActionResult> ChatIndex(string? userId, int postId)
    {
        var user = await _controllerHelper.GetUserAsync(User);
        if (user != null)
        {
            var allMessageBoxes = _messageService.GetAllMessageBoxes(user.Id).ToList();
            ChatViewModel viewModel = new() { AllChat = allMessageBoxes };
            ViewBag.LoginUserId = user.Id;

            if (userId != null)
            {
                ViewBag.ReceiverUserId = userId;
                ViewBag.PostId = postId;
                viewModel.CurrentChat = await _messageService.GetOrCreateMessageBox(userId, user.Id, postId); ;
            }
            return View(viewModel);
        }
        else
        {
            return Redirect($"/{UrlConstants.Login}");
        }
    }

    // TODO : add logging before each return
    [HttpPost, ActionName(EndpointConstants.SendMessage)]
    public async Task<ActionResult> SendMessage(Guid id, string message, int postId, int messageBoxId)
    {
        bool isUserExist = await _controllerHelper.IsUserExist(id.ToString());
        if (!isUserExist)
        {
            return Json("Kayıt bulunamadı.");
        }
        var currentUser = await _controllerHelper.GetUserAsync(User);
        if (id.ToString().Equals(currentUser.Id))
        {
            return Json("Kullanıcı kendisine mesaj gönderemez.");
        }
        try
        {
            await _messageService.SendAndBroadCastMessageAsync(currentUser, id, message, postId, messageBoxId);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

}