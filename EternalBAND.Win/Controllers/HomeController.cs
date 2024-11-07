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
using EternalBAND.Common;

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

    [HttpGet, Route("Anasayfa")]
    public async Task<IActionResult> MainPage()
    {
        return View(await _homeService.GetMainPageModel());
    }

    [HttpGet,Route("blogs/{seolink?}")]
    public async Task<IActionResult> Blogs(string? seolink = "", int pId = 1)
    {
        if (seolink == null)
        {
            seolink = "";
        }
        return View(await _homeService.Blogs(pId, seolink));
    }

    [HttpGet, Route("blog/{seoLink}")]
    public async Task<IActionResult> Blog(string? seoLink = "")
    {
        if (seoLink == null)
        {
            return RedirectToAction(nameof(Anasayfa));
        }

        return View(await _homeService.Blog(seoLink));
    }

    // TODO change parameter names as understandable strings
    [HttpPost,Route("ilanlar")]
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

    [HttpGet, Route("ilanlar")]
    public async Task<IActionResult> Posts(string s)
    {
        return await Posts(1, s);
    }

    [HttpGet, Route("ilan/{seolink}")]
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

    [HttpPost, Route("FilterNewPosts")]
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

    [HttpPost, Route("NewPosts")]
    public async Task<IActionResult> NewPosts()
    {
        var filteredPosts = await _homeService.NewPosts();
        return PartialView("PartialViews/_PostsListPartial", filteredPosts);
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


    [HttpGet,Route("KVKK")]
    public IActionResult Gdpr()
    {
        return View();
    }

    [HttpGet,Route("PrivacyPolicy")]
    public IActionResult PrivacyPolicy()
    {
        return View();
    }

    [HttpGet, Route("PostRules")]
    public IActionResult PostRules()
    {
        return View();
    }

    [HttpGet, Route("About")]
    public IActionResult About()
    {
        return View();
    }

    [HttpPost,Route("SendSupportMessage")]
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

    [HttpGet,ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}