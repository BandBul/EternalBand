using EternalBAND.Business;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Api.Exceptions;
using System.Net;
using EternalBAND.Common;

namespace EternalBAND.Controllers.User;
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class UserController : Controller
{
    private readonly ControllerHelper _controllerHelper;
    private readonly UserService _userService;

    public UserController(
        ControllerHelper controllerHelper,
        UserService userService)
    {
        _controllerHelper = controllerHelper;
        _userService = userService;
    }

    [HttpGet, Route(EndpointConstants.MyPosts)]
    public async Task<IActionResult> PostIndex()
    {
        ViewBag.PostStatusViewModel = Initials.InitialPostStatusViewModelValue;
        
        var currentUser = await _controllerHelper.GetUserAsync(User);
        var allPosts = await _userService.PostIndex(currentUser);
        
        return View(allPosts);
    }

    [HttpGet, Route(EndpointConstants.PostCreate)]
    public IActionResult PostCreate()
    {
        PrepareViewData();
        return View();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [ValidateAntiForgeryToken]
    [HttpPost, Route(EndpointConstants.PostCreate)]
    public async Task<IActionResult> PostCreate([Bind("Id,Title,HTMLText,PostTypesId,InstrumentsId,CityId")] Posts posts,
        List<IFormFile>? uploadedFiles)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _userService.PostCreate(currentUser, posts, uploadedFiles);
            return RedirectToAction(nameof(PostIndex));
        }
        else
        {
            PrepareViewData(posts.PostTypesId, posts.InstrumentsId, posts.CityId);
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
      
        return View(posts);
    }
    // TODO please pass PK Id not GUID : since we can use Find method to seek PK as a best practice
    [HttpGet, Route(EndpointConstants.PostEdit)]
    public async Task<IActionResult> PostEdit(Guid? id)
    {
        try
        {
            var post = await _userService.PostEditInitial(id);
            PrepareViewData();
            return View(post);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            // TODO log 
            throw;
        }
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [ValidateAntiForgeryToken]
    [HttpPost,Route(EndpointConstants.PostEdit)]
    public async Task<IActionResult> PostEdit(
        [Bind(
            "Guid,Title,HTMLText,PostTypesId,InstrumentsId,CityId")]
        Posts posts,
        List<IFormFile>? uploadedFiles,
        List<string>? deletedFilesIndex)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var currentUser = await _controllerHelper.GetUserAsync(User);
                await _userService.PostEdit(currentUser, posts, uploadedFiles, deletedFilesIndex);
                return RedirectToAction(nameof(PostIndex));
            }
           
            catch(NotFoundException)
            {
                return NotFound();
            }
        }
        else 
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            PrepareViewData(posts.PostTypesId, posts.InstrumentsId, posts.CityId);
        }
        return View(posts);
    }


    //[ValidateAntiForgeryToken]
    [HttpPost, ActionName(EndpointConstants.PostArchivedAction)]
    public async Task<IActionResult> PostArchived(int id)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _userService.PostArchived(currentUser, id);
            return Redirect("/ilanlarim");
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //[ValidateAntiForgeryToken]
    [HttpPost, ActionName(EndpointConstants.PostActivatedAction)]
    public async Task<IActionResult> PostActivated(int id)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _userService.PostActivated(currentUser, id);
            return Redirect("/ilanlarim");
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // TODO please pass PK Id not GUID : since we can use Find method to seek PK as a best practice
    [HttpPost, ActionName(EndpointConstants.PostDeleteAction)]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            var result = await _userService.PostDelete(currentUser, id);
            return Json(result);
        }
        catch (JsonException ex)
        {
            return Json(ex.Message);
        }
    }

    [HttpPost, ActionName(EndpointConstants.AllReadNotificationAction)]
    public async Task<JsonResult> AllReadNotification()
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            var result = await _userService.AllReadNotification(currentUser);
           
            return Json(result);
        }
        catch(Exception)
        {
            return Json("İşlem başarısız.");
        }
    
    }
    [HttpGet, Route(EndpointConstants.Notifications)]
    public async Task<IActionResult> NotificationIndex(int pId=1)
    {
        var currentUser = await _controllerHelper.GetUserAsync(User);
        var result = await _userService.NotificationIndex(currentUser, pId);
        return View(result);
    }

    [HttpGet, ActionName(EndpointConstants.NotificationReadAction)]
    public async Task<IActionResult> NotificationRead(int id)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            var redirectionLink = await _userService.NotificationRead(id, currentUser);
            return Redirect("/" + redirectionLink);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (JsonException ex)
        {
            TempData["WarningMessage"] = ex.Message;
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    private void PrepareViewData(int? postTypeId = null, int? instrumentsId = null, int? cityId = null)
    {
        var postTypes = new List<PostTypes>() { new PostTypes() { Active = true, Id = null, FilterText = "Seçiniz", Type = "Default" } };
        postTypes.AddRange(_userService.GetPostTypes());

        var instruments = new List<Instruments>() { new Instruments() { Id = null, Instrument = "Seçiniz", InstrumentShort = "Default" } };
        instruments.AddRange(_userService.GetInstruments());

        ViewData["PostTypesId"] = postTypeId != null ? new SelectList(postTypes, "Id", "FilterText", postTypeId) : new SelectList(postTypes, "Id", "FilterText");
        ViewData["InstrumentsId"] = instrumentsId != null ? new SelectList(instruments, "Id", "Instrument", instrumentsId) : new SelectList(instruments, "Id", "Instrument");
        ViewData["CityId"] = cityId != null ? new SelectList(Cities.GetCities(), "Id", "Name", cityId) : new SelectList(Cities.GetCities(), "Id", "Name");
    }
}