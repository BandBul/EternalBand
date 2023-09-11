using System.Diagnostics;
using EternalBAND.Data;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EternalBAND.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("")]
    public IActionResult Anasayfa()
    {
        return View();
    }

    [Route("blog-yazilari")]
    public async Task<IActionResult> Blogs(int pId = 1, string? s = "")
    {
        if (s == null) s = "";
        return View(await _context.Blogs.Where(n => n.Title.Contains(s)).ToPagedListAsync(pId, 10));
    }

    [Route("blog")]
    public async Task<IActionResult> Blog(string? s = "")
    {
        if (s == null)
            return RedirectToAction(nameof(Anasayfa));

        return View(await _context.Blogs.FirstOrDefaultAsync(n => n.SeoLink == s));
    }

    [Route("ilanlar")]
    public async Task<IActionResult> Posts(int pId = 1, string? s = "", int c = 0, string? e = "")
    {
        ViewBag.CityId = c;
        ViewBag.TypeShort = s;
        ViewBag.Instrument = e;
        var r = await _context.Posts.Where(p => p.Status == Common.PostStatus.Active).Include(n => n.PostTypes).Include(n => n.Instruments).ToListAsync();
        if (c != 0 || e != "" || s != "")
        {
            if(e != "0" && s!= "0")
            {
                r = r.Where(n => n.Instruments.InstrumentShort.Contains(e) || n.PostTypes.TypeShort.Contains(s))
                    .ToList();
            }
            else if (e != "0")
            {
                r = r.Where(n => n.Instruments.InstrumentShort.Contains(e))
                    .ToList();
            }
            else if (s != "0")
            {
                r = r.Where(n =>n.PostTypes.TypeShort.Contains(s))
                    .ToList();
            }
            if (c != 0)
            {
                r = r.Where(n => n.CityId == c).ToList();
            }
            return View(await r.ToPagedListAsync(pId,10));
        }

        return View(await r.ToPagedListAsync(pId, 10));
    }

    [Route("ilan")]
    public async Task<IActionResult> Post(string? s = "")
    {
        if (s == null)
            return RedirectToAction(nameof(Anasayfa));

        return View(await _context.Posts.Include(n => n.PostTypes).Include(n => n.Instruments)
            .FirstOrDefaultAsync(n => n.SeoLink == s));
    }

    [Route("iletisim")]
    public IActionResult ContactsCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("iletisim")]
    public async Task<IActionResult> ContactsCreate(
        [Bind("Id,Mail,Phone,NameSurname,Title,Message,AddedDate")] Contacts contacts)
    {
        if (ModelState.IsValid)
        {
            contacts.AddedDate = DateTime.Now;

            _context.Add(contacts);
            await _context.SaveChangesAsync();
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