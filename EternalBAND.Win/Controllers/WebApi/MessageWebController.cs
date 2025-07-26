using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Common;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Win.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireUserRole")]
    public class MessageWebController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly ControllerHelper _controllerHelper;

        public MessageWebController(MessageService messageService, ControllerHelper controllerHelper)
        {
            _messageService = messageService;
            _controllerHelper = controllerHelper;
        }

        [HttpGet(EndpointConstants.MessagesEndpoint)]
        // userId : message receiver userId
        // postId : mssage is being sent for related postId
        public async Task<ActionResult> ChatIndex(string? userId, int postId)
        {
            var user = await _controllerHelper.GetUserAsync(User);
            if(user != null)
            {
                var allMessageBoxes = _messageService.GetAllMessageBoxes(user.Id).ToList();
                ChatViewModel viewModel = new() { AllChat = allMessageBoxes };

                if (userId != null)
                {
                    viewModel.CurrentChat = await _messageService.GetOrCreateMessageBox(userId, user.Id, postId); ;
                }
                return Ok(viewModel);
            }
            return Problem("Unknown user", null, 404);
          
        }

        // TODO : add logging before each return
        [HttpPost(EndpointConstants.SendMessage)]
        public async Task<ActionResult> SendMessage(Guid id, string message, int postId, int messageBoxId)
        {
            bool isUserExist = await _controllerHelper.IsUserExist(id.ToString());
            if (!isUserExist)
            {
                return Problem("User not found", null, 404);
            }
            var currentUser = await _controllerHelper.GetUserAsync(User);
            if (id.ToString().Equals(currentUser.Id))
            {
                return Problem("User can not send message to yourself");
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
}
