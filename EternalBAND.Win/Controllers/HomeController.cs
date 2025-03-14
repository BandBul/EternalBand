﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using EternalBAND.Api.Services;
using EternalBAND.Api.Helpers;
using EternalBAND.Common;
using EternalBAND.Api;
using Newtonsoft.Json.Linq;

namespace EternalBAND.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HomeService _homeService;
    private readonly ControllerHelper _controllerHelper;

    public HomeController(ILogger<HomeController> logger, HomeService homeService, ControllerHelper controllerHelper)
    {
        _logger = logger;
        _homeService = homeService;
        _controllerHelper = controllerHelper;
    }
    [HttpGet, Route("")]
    public async Task<IActionResult> Anasayfa()
    {
        return View(await _homeService.GetMainPageModel());
    }

    [HttpGet, Route(EndpointConstants.Anasayfa)]
    public async Task<IActionResult> MainPage()
    {
        return View(await _homeService.GetMainPageModel());
    }

    [HttpGet,Route(EndpointConstants.BlogsEndpoint)]
    public async Task<IActionResult> Blogs(string? seolink = "", int pId = 1)
    {
        if (seolink == null)
        {
            seolink = "";
        }
        return View(await _homeService.Blogs(pId, seolink));
    }

    [HttpGet, Route(EndpointConstants.Blog)]
    public async Task<IActionResult> Blog(string? seoLink = "")
    {
        if (seoLink == null)
        {
            return RedirectToAction(nameof(Anasayfa));
        }

        return View(await _homeService.Blog(seoLink));
    }

    // TODO change parameter names as understandable strings
    [Route(EndpointConstants.Posts)]
    public async Task<IActionResult> Posts(int pId = 1, string? s = "0", int c = 0, string? e = "0")
    {
        ViewBag.CityId = c;
        ViewBag.TypeShort = s;
        ViewBag.Instrument = e;
        var model = new PostViewModel()
        {
            Posts = await _homeService.FilterPostsByType(pId, s, c, e),
            PostFilterContracts = new PostFilterContracts()
            {
                PageID = pId,
                CityId = c,
                Instrument = e,
                Type = s,

            }
        };
        return View(model);
    }

    [HttpGet, Route(EndpointConstants.Post)]
    public async Task<IActionResult> Post(string? seolink , bool approvalPurpose = false)
    {
        if (approvalPurpose)
        {
            var isAdmin = await _controllerHelper.IsUserAdmin(User);
            if(!isAdmin)
            {
                approvalPurpose = false;
            }
        }
        if (seolink == null)
        {
            return RedirectToAction(nameof(Anasayfa));
        }
        ViewBag.ApprovalPurpose = approvalPurpose;
        return View(await _homeService.Post(seolink));
    }

    [HttpGet, Route(EndpointConstants.Contact)]
    public IActionResult ContactsCreate()
    {
        return View();
    }

    [HttpPost, Route(EndpointConstants.FilterNewPosts)]
    public async Task<IActionResult> FilterNewPosts(string postTypeName)
    {
        PostTypeName postType;
        if(!Enum.TryParse(postTypeName, out postType))
        {
            return Problem("Invalid posttype value '{}'", postTypeName);
        }

        var filteredPosts = await _homeService.PostsByPostTypeAsync(postType);
        return PartialView("PartialViews/_PostsListPartial", filteredPosts);
    }
    // TODO : Change name as RecentPosts
    [HttpPost, Route(EndpointConstants.NewPosts)]
    public async Task<IActionResult> NewPosts()
    {
        var filteredPosts = await _homeService.GetRecentPosts();
        return PartialView("PartialViews/_PostsListPartial", filteredPosts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route(EndpointConstants.Contact)]
    public async Task<IActionResult> ContactsCreate(
        [Bind("Id,Mail,Phone,NameSurname,Title,Message,AddedDate")] Contacts contacts)
    {
        if (ModelState.IsValid)
        {
            await _homeService.ContactsCreate(contacts);
            ViewBag.Message = "İletişim formunuz başarıyla ulaştı.";
        }
        else
        {
            ViewBag.Message = "İletişim formunuz iletilirken hata ile karşılaşıldı.";
        }

        return View(contacts);
    }


    [HttpGet,Route(EndpointConstants.Kvkk)]
    public IActionResult Gdpr()
    {
        return View();
    }

    [HttpGet,Route(EndpointConstants.PrivacyPolicy)]
    public IActionResult PrivacyPolicy()
    {
        return View();
    }

    [HttpGet, Route(EndpointConstants.PostRules)]
    public IActionResult PostRules()
    {
        return View();
    }

    [HttpGet, Route(EndpointConstants.About)]
    public IActionResult About()
    {
        return View();
    }

    [HttpPost,Route(EndpointConstants.SendSupportMessage)]
    [Authorize]
    public async Task<IActionResult> SendSupportMessage(string message)
    {
        var contractMessage = new Contacts()
        {
            Message = message
        };

        if (User != null) 
        {
            var user = await _controllerHelper.GetUserAsync(User);
            contractMessage.NameSurname = user.FullName;
            contractMessage.Mail = user.Email;
            contractMessage.Phone = user.PhoneNumber;
        }
        await ContactsCreate(contractMessage);

        //TO DO add a message that your message is received to us
        // ViewBag.IsContactMessageReceived = ......
        // show this info as banner when page redirected
        return RedirectToAction(nameof(Anasayfa));
    }

    [HttpGet, Route(EndpointConstants.Error),ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}