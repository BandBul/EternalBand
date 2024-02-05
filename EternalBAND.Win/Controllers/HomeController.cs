using System.Diagnostics;
using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using EternalBAND.Api.Services;
using System.Security.Cryptography;
using EternalBAND.Api.Helpers;

namespace EternalBAND.Controllers;
[Authorize]
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
    [AllowAnonymous]
    [HttpGet, Route("")]
    public IActionResult Anasayfa()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet, Route("Anasayfa")]
    public IActionResult MainPage()
    {
        return View();
    }

    [HttpGet, Route("blog-yazilari")]
    public async Task<IActionResult> Blogs(int pId = 1, string? s = "")
    {
        if (s == null)
        {
            s = "";
        }
        return View(await _homeService.Blogs(s, pId));
    }

    [HttpGet, Route("blog")]
    public async Task<IActionResult> Blog(string? s = "")
    {
        if (s == null)
        {
            return RedirectToAction(nameof(Anasayfa));
        }

        return View(await _homeService.Blog(s));
    }
    // TODO change parameter names as understandable strings
    [HttpGet, Route("ilanlar")]
    public async Task<IActionResult> Posts(int pId = 1, string? s = "", int c = 0, string? e = "")
    {
        ViewBag.CityId = c;
        ViewBag.TypeShort = s;
        ViewBag.Instrument = e;
        return View(await _homeService.Posts(pId, s, c, e));
    }

    [HttpGet, Route("ilan/{seolink}")]
    public async Task<IActionResult> Post(string? seolink , bool approvalPurpose = false)
    {
        if (approvalPurpose)
        {
            var isAdmin = await _controllerHelper.IsUserAdmin(User);
            if(isAdmin)
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

    [HttpGet, Route("iletisim")]
    public IActionResult ContactsCreate()
    {
        return View();
    }

    [HttpGet, Route("SeeAllPost")]
    public async Task<IActionResult> SeeAllPost()
    {
        return Redirect("/ilanlar");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("iletisim")]
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}