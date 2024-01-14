using EternalBAND.Common;
using EternalBAND.Data;
using EternalBAND.Helpers;
using EternalBAND.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EternalBAND.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Users> _userManager;
    private readonly IWebHostEnvironment _environment;

    public AdminController(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _userManager = userManager;
    }

    // GET: Blogs
    public async Task<IActionResult> BlogsIndex(int pId = 1)
    {
        return _context.Blogs != null
            ? View(await _context.Blogs.OrderByDescending(n=> n.Id).ToPagedListAsync(pId,10))
            : Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
    }


    // GET: Blogs/Create
    public IActionResult BlogsCreate()
    {
        return View();
    }

    // POST: Blogs/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlogsCreate(
        [Bind("Id,Title,HtmlText,Tags,SeoLink")]
        Blogs blogs,
        List<IFormFile>? images)
    {
        if (ModelState.IsValid)
        {
            BlogPhotoAdd(blogs, images);
            blogs.SeoLink = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(blogs.Title) + "-" + new Random().Next(0, 999999);
            blogs.AddedDate = DateTime.Now;
            _context.Add(blogs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BlogsIndex));
        }

        return View(blogs);
    }

    // GET: Blogs/Edit/5
    public async Task<IActionResult> BlogsEdit(int? id)
    {
        if (id == null || _context.Blogs == null)
        {
            return NotFound();
        }

        var blogs = await _context.Blogs.FindAsync(id);
        if (blogs == null)
        {
            return NotFound();
        }

        return View(blogs);
    }

    // POST: Blogs/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlogsEdit(int id,
        [Bind("Id,Title,HtmlText,Tags,SeoLink")]
        Blogs blogs, List<IFormFile?> images)
    {
        if (id != blogs.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            BlogPhotoAdd(blogs, images);
            var blog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(n => n.Id == blogs.Id);

            if (images.Count == 0)
            {
                blogs.PhotoPath = blog.PhotoPath;
                blogs.PhotoPath2 = blog.PhotoPath2;
                blogs.PhotoPath3 = blog.PhotoPath3;
                blogs.PhotoPath4 = blog.PhotoPath4;
                blogs.PhotoPath5 = blog.PhotoPath5;
            }

            try
            {
                blogs.AddedDate = blog.AddedDate;
                _context.Update(blogs);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogsExists(blogs.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(BlogsIndex));
        }

        return View(blogs);
    }


    // POST: Blogs/Delete/5
    [HttpPost, ActionName("BlogsDelete")]
    public async Task<IActionResult> BlogsDeleteConfirmed(int id)
    {
        if (_context.Blogs == null)
        {
            return Json("Kayıt bulunamadı.");
        }

        var blogs = await _context.Blogs.FindAsync(id);
        if (blogs != null)
        {
            _context.Blogs.Remove(blogs);
        }

        await _context.SaveChangesAsync();
        return Json("Blog yazısı silindi.");
    }

    private bool BlogsExists(int id)
    {
        return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    async void BlogPhotoAdd(Blogs blogs, List<IFormFile?> images)
    {
        if (images != null)
        {
            foreach (var image in images)
            {
                var photoName = new PhotoName().GeneratePhotoName(new Random().Next(0, 10000000).ToString()) +
                                new Random().Next(0, 1000) +
                                System.IO.Path.GetExtension(image.FileName);
                using (var stream =
                       new FileStream(
                           Path.Combine(_environment.WebRootPath, "images/ilan/", photoName),
                           FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                    if (blogs.PhotoPath == null)
                    {
                        blogs.PhotoPath = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (blogs.PhotoPath3 == null)
                    {
                        blogs.PhotoPath3 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (blogs.PhotoPath3 == null)
                    {
                        blogs.PhotoPath3 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (blogs.PhotoPath4 == null)
                    {
                        blogs.PhotoPath4 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (blogs.PhotoPath5 == null)
                    {
                        blogs.PhotoPath5 = "/images/ilan/" + photoName;
                        continue;
                    }
                }
            }
        }
    }

    // GET: Contacts
    public async Task<IActionResult> ContactsIndex(int pId =1)
    {
        return _context.Contacts != null
            ? View(await _context.Contacts.OrderByDescending(n=> n.Id).ToPagedListAsync(pId,10))
            : Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
    }

    // GET: Contacts/Details/5
    public async Task<IActionResult> ContactsDetails(int? id)
    {
        if (id == null || _context.Contacts == null)
        {
            return NotFound();
        }

        var contacts = await _context.Contacts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contacts == null)
        {
            return NotFound();
        }

        return View(contacts);
    }


    // GET: Contacts/Edit/5
    public async Task<IActionResult> ContactsEdit(int? id)
    {
        if (id == null || _context.Contacts == null)
        {
            return NotFound();
        }

        var contacts = await _context.Contacts.FindAsync(id);
        if (contacts == null)
        {
            return NotFound();
        }

        return View(contacts);
    }

    // POST: Contacts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsEdit(int id,
        [Bind("Id,Mail,Phone,NameSurname,Title,Message,AddedDate,IsDone")]
        Contacts contacts)
    {
        if (id != contacts.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contacts);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsExists(contacts.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(ContactsIndex));
        }

        return View(contacts);
    }

    // GET: Contacts/Delete/5
    public async Task<IActionResult> ContactsDelete(int? id)
    {
        if (id == null || _context.Contacts == null)
        {
            return NotFound();
        }

        var contacts = await _context.Contacts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contacts == null)
        {
            return NotFound();
        }

        return View(contacts);
    }

    // POST: Contacts/Delete/5
    [HttpPost, ActionName("ContactsDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsDeleteConfirmed(int id)
    {
        if (_context.Contacts == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
        }

        var contacts = await _context.Contacts.FindAsync(id);
        if (contacts != null)
        {
            _context.Contacts.Remove(contacts);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ContactsIndex));
    }

    private bool ContactsExists(int id)
    {
        return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // GET: PostTypes
    public async Task<IActionResult> PostTypesIndex(int pId=1)
    {
        return _context.PostTypes != null
            ? View(await _context.PostTypes.OrderByDescending(n=>n.Id).ToPagedListAsync(pId,10))
            : Problem("Entity set 'ApplicationDbContext.PostTypes'  is null.");
    }


    // GET: PostTypes/Create
    public IActionResult PostTypesCreate()
    {
        return View();
    }

    // POST: PostTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostTypesCreate([Bind("Id,Type")] PostTypes postTypes)
    {
        if (ModelState.IsValid)
        {
            postTypes.TypeShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(postTypes.Type);
            postTypes.Active = true;
            postTypes.AddedDate = DateTime.Now;
            _context.Add(postTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PostTypesIndex));
        }

        return View(postTypes);
    }

    // GET: PostTypes/Edit/5
    public async Task<IActionResult> PostTypesEdit(int? id)
    {
        if (id == null || _context.PostTypes == null)
        {
            return NotFound();
        }

        var postTypes = await _context.PostTypes.FindAsync(id);
        if (postTypes == null)
        {
            return NotFound();
        }

        return View(postTypes);
    }

    // POST: PostTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostTypesEdit(int id, [Bind("Id,Type,Active")] PostTypes postTypes)
    {
        if (id != postTypes.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var post = await _context.PostTypes.FirstOrDefaultAsync(n => n.Id == id);
                post.Active = postTypes.Active;
                post.Type = postTypes.Type;
                post.TypeShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(postTypes.Type);
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostTypesExists(postTypes.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(PostTypesIndex));
        }

        return View(postTypes);
    }

    // GET: PostTypes/Delete/5
    public async Task<IActionResult> PostTypesDelete(int? id)
    {
        if (id == null || _context.PostTypes == null)
        {
            return NotFound();
        }

        var postTypes = await _context.PostTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (postTypes == null)
        {
            return NotFound();
        }

        return View(postTypes);
    }

    // POST: PostTypes/Delete/5
    [HttpPost, ActionName("PostTypesDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostTypesDeleteConfirmed(int id)
    {
        if (_context.PostTypes == null)
        {
            return Problem("Entity set 'ApplicationDbContext.PostTypes'  is null.");
        }

        var postTypes = await _context.PostTypes.FindAsync(id);
        if (postTypes != null)
        {
            _context.PostTypes.Remove(postTypes);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(PostTypesIndex));
    }

    private bool PostTypesExists(int id)
    {
        return (_context.PostTypes?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // GET: SystemInfo
    public async Task<IActionResult> SystemInfoIndex(int pId =1)
    {
        return _context.SystemInfo != null
            ? View(await _context.SystemInfo.OrderByDescending(n=> n.Id).ToPagedListAsync(pId,10))
            : Problem("Entity set 'ApplicationDbContext.SystemInfo'  is null.");
    }


    // GET: SystemInfo/Create
    public IActionResult SystemInfoCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SystemInfoCreate([Bind("Id,Type,Value,Desc")] SystemInfo systemInfo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(systemInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SystemInfoIndex));
        }

        return View(systemInfo);
    }

    // GET: SystemInfo/Edit/5
    public async Task<IActionResult> SystemInfoEdit(int? id)
    {
        if (id == null || _context.SystemInfo == null)
        {
            return NotFound();
        }

        var systemInfo = await _context.SystemInfo.FindAsync(id);
        if (systemInfo == null)
        {
            return NotFound();
        }

        return View(systemInfo);
    }

    // POST: SystemInfo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SystemInfoEdit(int id, [Bind("Id,Type,Value,Desc")] SystemInfo systemInfo,
        IFormFile? image)
    {
        if (id != systemInfo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (systemInfo.Type == "site-title")
                {
                    Business.AppSettings.SetAppSettingValue("SiteName", systemInfo.Value);
                } 
                if (systemInfo.Type == "site-domain")
                {
                    Business.AppSettings.SetAppSettingValue("SiteDomain", systemInfo.Value);
                }

                if (systemInfo.Type == "site-logo" || systemInfo.Type == "site-favicon")
                {
                  

                    if (image != null)
                    { 
                        var photoName =new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(image.FileName)+ new PhotoName().GeneratePhotoName(new Random().Next(0, 10000000).ToString()) +
                                        new Random().Next(0, 1000) +
                                        System.IO.Path.GetExtension(image.FileName);
                        using (var stream =
                               new FileStream(
                                   Path.Combine(_environment.WebRootPath, "images/logo/", photoName),
                                   FileMode.Create))

                        {
                            await image.CopyToAsync(stream);
                        }

                        systemInfo.Value = "/images/logo/" + photoName;
                        if (systemInfo.Type == "site-logo")
                        {
                            Business.AppSettings.SetAppSettingValue("SiteLogo", systemInfo.Value);
                        }

                        if (systemInfo.Type == "site-favicon")
                        {
                            Business.AppSettings.SetAppSettingValue("SiteFavicon", systemInfo.Value);
                        }
                       
                    }
                }

                _context.Update(systemInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemInfoExists(systemInfo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(SystemInfoIndex));
        }

        return View(systemInfo);
    }

    // GET: SystemInfo/Delete/5
    public async Task<IActionResult> SystemInfoDelete(int? id)
    {
        if (id == null || _context.SystemInfo == null)
        {
            return NotFound();
        }

        var systemInfo = await _context.SystemInfo
            .FirstOrDefaultAsync(m => m.Id == id);
        if (systemInfo == null)
        {
            return NotFound();
        }

        return View(systemInfo);
    }

    // POST: SystemInfo/Delete/5
    [HttpPost, ActionName("SystemInfoDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SystemInfoDeleteConfirmed(int id)
    {
        if (_context.SystemInfo == null)
        {
            return Problem("Entity set 'ApplicationDbContext.SystemInfo'  is null.");
        }

        var systemInfo = await _context.SystemInfo.FindAsync(id);
        if (systemInfo != null)
        {
            _context.SystemInfo.Remove(systemInfo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(SystemInfoIndex));
    }

    private bool SystemInfoExists(int id)
    {
        return (_context.SystemInfo?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // GET: Instruments
    public async Task<IActionResult> InstrumentsIndex(int pId=1)
    {
        return _context.Instruments != null
            ? View(await _context.Instruments.OrderByDescending(n=> n.Id).ToPagedListAsync(pId,10))
            : Problem("Entity set 'ApplicationDbContext.Instruments'  is null.");
    }


    // GET: Instruments/Create
    public IActionResult InstrumentsCreate()
    {
        return View();
    }

    // POST: Instruments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstrumentsCreate([Bind("Id,Instrument")] Instruments instruments)
    {
        if (ModelState.IsValid)
        {
            instruments.InstrumentShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
            instruments.IsActive = true;
            _context.Add(instruments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(InstrumentsIndex));
        }

        return View(instruments);
    }

    // GET: Instruments/Edit/5
    public async Task<IActionResult> InstrumentsEdit(int? id)
    {
        if (id == null || _context.Instruments == null)
        {
            return NotFound();
        }

        var instruments = await _context.Instruments.FindAsync(id);
        if (instruments == null)
        {
            return NotFound();
        }

        return View(instruments);
    }

    // POST: Instruments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstrumentsEdit(int id, [Bind("Id,Instrument,IsActive")] Instruments instruments)
    {
        if (id != instruments.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                instruments.InstrumentShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
                _context.Update(instruments);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstrumentsExists(instruments.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(InstrumentsIndex));
        }

        return View(instruments);
    }

    // GET: Instruments/Delete/5
    public async Task<IActionResult> InstrumentsDelete(int? id)
    {
        if (id == null || _context.Instruments == null)
        {
            return NotFound();
        }

        var instruments = await _context.Instruments
            .FirstOrDefaultAsync(m => m.Id == id);
        if (instruments == null)
        {
            return NotFound();
        }

        return View(instruments);
    }

    // POST: Instruments/Delete/5
    [HttpPost, ActionName("InstrumentsDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstrumentsDeleteConfirmed(int id)
    {
        if (_context.Instruments == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Instruments'  is null.");
        }

        var instruments = await _context.Instruments.FindAsync(id);
        if (instruments != null)
        {
            _context.Instruments.Remove(instruments);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(InstrumentsIndex));
    }

    [ActionName("ApprovePost")]
    // TODO-Engin check token validation
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePost( string postSeoLink)
    {
        try
        {
            var post = _context.Posts.Where(p => p.SeoLink.Equals(postSeoLink)).FirstOrDefault();
            post.Status = PostStatus.Active;
            _context.Update(post);

            // TODO-Engin getting all seolink of current post we need to add an check also received user is admin or not
            var allNotifOnAdmin = _context.Notification.Where(not => not.RelatedElementId.Equals(postSeoLink) && not.IsRead == false).ToList();
            allNotifOnAdmin.ForEach(n =>
            {
                n.IsRead = true;
            });

            _context.UpdateRange(allNotifOnAdmin);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: Constants.MainPage, controllerName: "Home");
        }
        catch (Exception ex )
        {
            // TO DO 
            // logger.LogError(ex,"Problem happens during approving the post. {ex.Message}");
            throw;
        }

    }

    private bool InstrumentsExists(int id)
    {
        return (_context.Instruments?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}