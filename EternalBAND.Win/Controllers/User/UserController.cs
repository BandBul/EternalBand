using EternalBAND.Business;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace EternalBAND.Controllers.User;
// TODO create a Reposıtory for manage _context, controller should only responsible for API and basic validations 
[Authorize]
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

    [HttpGet, Route("ilanlarim")]
    public async Task<IActionResult> PostIndex()
    {
        ViewBag.PostStatusViewModel = Initials.InitialPostStatusViewModelValue;
        
        var currentUser = await _controllerHelper.GetUserAsync(User);
        var allPosts = await _userService.PostIndex(currentUser);
        
        return View(allPosts);
    }

    [HttpGet, Route("PostCreate")]
    public IActionResult PostCreate()
    {
        var postTypes = new List<PostTypes>() { new PostTypes() { Active = true, Id = null, Type = "Seçiniz", TypeShort = "Default" } };
        postTypes.AddRange(_userService.GetPostTypes());

        var instruments = new List<Instruments>() { new Instruments() { Id = null, Instrument = "Seçiniz", InstrumentShort = "Default" } };
        instruments.AddRange(_userService.GetInstruments());

        ViewData["PostTypesId"] = new SelectList(postTypes, "Id", "Type");
        ViewData["InstrumentsId"] = new SelectList(instruments, "Id", "Instrument");
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Id", "Name");
        return View();
    }

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [ValidateAntiForgeryToken]
    [HttpPost, Route("PostCreate")]
    public async Task<IActionResult> PostCreate([Bind("Id,Title,HTMLText,PostTypesId,InstrumentsId,CityId")] Posts posts,
        List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _userService.PostCreate(currentUser, posts, images);
            return RedirectToAction(nameof(PostIndex));
        }
        else
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            // TODO what should we do with this error list we can display it at FE
        }
      
        return View(posts);
    }
    // TODO please pass PK Id not GUID : since we can use Find method to seek PK as a best practice
    // GET: Posts/Edit/5
    [HttpGet, Route("ilan-duzenle")]
    public async Task<IActionResult> PostEdit(Guid? id)
    {
        try
        {
            var post = await _userService.PostEditInitial(id);
            ViewData["PostTypesId"] = new SelectList(_userService.GetPostTypes(), "Id", "Type", post.PostTypesId);
            ViewData["InstrumentsId"] = new SelectList(_userService.GetInstruments(), "Id", "Instrument", post.InstrumentsId);
            ViewData["CityId"] = new SelectList(Cities.GetCities(), "Id", "Name", post.CityId);
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

    // POST: Posts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [ValidateAntiForgeryToken]
    [HttpPost,Route("ilan-duzenle")]
    public async Task<IActionResult> PostEdit(
        [Bind(
            "Guid,Title,HTMLText,PostTypesId,InstrumentsId,CityId")]
        Posts posts,
        List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var currentUser = await _controllerHelper.GetUserAsync(User);
                await _userService.PostEdit(currentUser, posts, images);
                return RedirectToAction(nameof(PostIndex));
            }
           
            catch(NotFoundException)
            {
                return NotFound();
            }
            
        }

        ViewData["PostTypesId"] = new SelectList(_userService.GetPostTypes(), "Id", "Type", posts.PostTypesId);
        ViewData["InstrumentsId"] = new SelectList(_userService.GetInstruments(), "Id", "Instrument", posts.InstrumentsId);
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Id", "Name", posts.CityId);
        return View(posts);
    }


    //[ValidateAntiForgeryToken]
    [HttpPost, ActionName("PostArchived")]
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
    // TODO it should be POST
    //[ValidateAntiForgeryToken]
    [HttpPost, ActionName("PostActivated")]
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
    [HttpPost, ActionName("PostDelete")]
    public async Task<JsonResult> DeleteConfirmed(Guid id)
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

    [HttpPost, ActionName("AllReadNotification")]
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
    [Route("bildirimler")]
    public async Task<IActionResult> NotificationIndex(int pId=1)
    {
        var currentUser = await _controllerHelper.GetUserAsync(User);
        var result = await _userService.NotificationIndex(currentUser, pId);
        return View(result);
    }

    [ActionName("NotificationRead")]
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
}