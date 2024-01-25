using EternalBAND.Business;
using EternalBAND.Common;
using EternalBAND.DomainObjects;
using EternalBAND.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using EternalBAND.DataAccess;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using Microsoft.Extensions.Hosting;
using EternalBAND.Api.Exceptions;
using System;

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

    // GET: Posts
    [Route("ilanlarim")]
    public async Task<IActionResult> PostIndex()
    {
        ViewBag.PostStatusViewModel = Initials.InitialPostStatusViewModelValue;
        
        var currentUser = await _controllerHelper.GetUserAsync(User);
        var allPosts = _userService.PostIndex(currentUser);
        
        return View(allPosts);
    }

    [Route("PostCreate")]
    // GET: Posts/Create
    public IActionResult PostCreate()
    {
        ViewData["PostTypesId"] = new SelectList(_userService.GetPostTypes(), "Id", "Type");
        ViewData["InstrumentsId"] = new SelectList(_userService.GetInstruments(), "Id", "Instrument");
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value");
        return View();
    }

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("PostCreate")]
    public async Task<IActionResult> PostCreate([Bind("Id,Title,HTMLText,PostTypesId,InstrumentsId,CityId")] Posts posts,
        List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _userService.PostCreate(currentUser, posts, images);
            return RedirectToAction(nameof(PostIndex));
        }
        // TODO log warning and use allErrors to pass to Front End if needed
        else
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            ViewData["PostTypesId"] = new SelectList(_userService.GetPostTypes(), "Id", "Type", posts.PostTypesId);
            ViewData["InstrumentsId"] = new SelectList(_userService.GetInstruments(), "Id", "Instrument", posts.InstrumentsId);
            ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", posts.CityId);
        }
      
        return View(posts);
    }
    // TODO please pass PK Id not GUID : since we can use Find method to seek PK as a best practice
    // GET: Posts/Edit/5
    [Route("ilan-duzenle")]
    public async Task<IActionResult> PostEdit(Guid? id)
    {
        try
        {
            var post = await _userService.PostEditInitial(id);
            ViewData["PostTypesId"] = new SelectList(_userService.GetPostTypes(), "Id", "Type", post.PostTypesId);
            ViewData["InstrumentsId"] = new SelectList(_userService.GetInstruments(), "Id", "Instrument", post.InstrumentsId);
            ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", post.CityId);
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("ilan-duzenle")]
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
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", posts.CityId);
        return View(posts);
    }


    //[ValidateAntiForgeryToken]
    [ActionName("PostArchived")]
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
    [ActionName("PostActivated")]
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
            var result = _userService.PostDelete(currentUser, id);
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
            var redirectionLink = await _userService.NotificationRead(id);
            return Redirect("/" + redirectionLink);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}