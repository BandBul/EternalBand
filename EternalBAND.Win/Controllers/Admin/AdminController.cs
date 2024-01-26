using EternalBAND.Api.Exceptions;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AdminService _adminService;
    private readonly ControllerHelper _controllerHelper;

    public AdminController(AdminService adminService, ControllerHelper controllerHelper)
    {
        _adminService = adminService;
        _controllerHelper = controllerHelper;
    }

    // GET: Blogs
    public async Task<IActionResult> BlogsIndex(int pId = 1)
    {
        try
        {
            var blogs = await _adminService.BlogsIndex(pId);
            return View(blogs);
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
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
            await _adminService.BlogsCreate(blogs, images);
            return RedirectToAction(nameof(BlogsIndex));
        }
        else
        {
            return View(blogs);
        }
    }

    // GET: Blogs/Edit/5
    public async Task<IActionResult> BlogsEdit(int? id)
    {
        try
        {
            var blog = await _adminService.BlogsEditInitial(id);
            return View(blog);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
    // TODO please check this metod it may not work
    // POST: Blogs/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlogsEdit(int id,
        [Bind("Id,Title,HtmlText,Tags,SeoLink")]
        Blogs blogs, List<IFormFile?> images)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _adminService.BlogsEdit(id, blogs, images);
                return RedirectToAction(nameof(BlogsIndex));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        else
        {
            return View(blogs);
        }
    }


    // POST: Blogs/Delete/5
    [HttpPost, ActionName("BlogsDelete")]
    public async Task<IActionResult> BlogsDeleteConfirmed(int id)
    {
        try
        {
            var result = await _adminService.BlogsDeleteConfirmed(id);
            return Json(result);
        }
        catch (JsonException ex)
        {
            return Json(ex.Message);
        }
    }


    // GET: Contacts
    public async Task<IActionResult> ContactsIndex(int pId =1)
    {
        try
        {
            var contacts = await _adminService.ContactsIndex(pId);
            return View(contacts);
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
    }

    // GET: Contacts/Details/5
    public async Task<IActionResult> ContactsDetails(int? id)
    {
        try
        {
            var contacts = await _adminService.GetContacts(id);
            return View(contacts);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }


    // GET: Contacts/Edit/5
    public async Task<IActionResult> ContactsEdit(int? id)
    {
        try
        {
            var contacts = await _adminService.GetContacts(id);
            return View(contacts);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
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
        if (ModelState.IsValid)
        {
            try
            {
                await _adminService.ContactEdit(id, contacts);
                return RedirectToAction(nameof(ContactsIndex));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        else
        {
            return View(contacts);
        }
    }

    // GET: Contacts/Delete/5
    public async Task<IActionResult> ContactsDelete(int? id)
    {
        try
        {
            var contacts = await _adminService.GetContacts(id);
            return View(contacts);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: Contacts/Delete/5
    [HttpPost, ActionName("ContactsDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsDeleteConfirmed(int id)
    {
        try
        {
            await _adminService.ContactsDeleteConfirmed(id);
            return RedirectToAction(nameof(ContactsIndex));
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
    }



    // GET: PostTypes
    public async Task<IActionResult> PostTypesIndex(int pId=1)
    {
        try
        {
            var postTypes = await _adminService.PostTypesIndex(pId);
            return View(postTypes);
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
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
            await _adminService.PostTypesCreate(postTypes);
            return RedirectToAction(nameof(PostTypesIndex));
        }
        else
        {
            return View(postTypes);
        }
    }

    // GET: PostTypes/Edit/5
    public async Task<IActionResult> PostTypesEdit(int? id)
    {
        try
        {
            var postType = await _adminService.GetPostType(id);
            return View(postType);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: PostTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostTypesEdit(int id, [Bind("Id,Type,Active")] PostTypes postTypes)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _adminService.PostTypesEdit(id, postTypes);
                return RedirectToAction(nameof(PostTypesIndex));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        else
        {
            return View(postTypes);
        }
    }

    // GET: PostTypes/Delete/5
    public async Task<IActionResult> PostTypesDelete(int? id)
    {
        try
        {
            var postType = await _adminService.GetPostType(id);
            return View(postType);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: PostTypes/Delete/5
    [HttpPost, ActionName("PostTypesDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostTypesDeleteConfirmed(int id)
    {
        try
        {
            await _adminService.PostTypesDeleteConfirmed(id);
            return RedirectToAction(nameof(PostTypesIndex));
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
    }

    // GET: SystemInfo
    public async Task<IActionResult> SystemInfoIndex(int pId =1)
    {
        try
        {
            var systemInfo = await _adminService.BlogsIndex(pId);
            return View(systemInfo);
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
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
            await _adminService.SystemInfoCreate(systemInfo);
            return RedirectToAction(nameof(SystemInfoIndex));
        }
        else
        {
            return View(systemInfo);
        }
    }

    // GET: SystemInfo/Edit/5
    public async Task<IActionResult> SystemInfoEdit(int? id)
    {
        try
        {
            var systemInfo = await _adminService.GetSystemInfo(id);
            return View(systemInfo);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: SystemInfo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SystemInfoEdit(int id, [Bind("Id,Type,Value,Desc")] SystemInfo systemInfo,
        IFormFile? image)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _adminService.SystemInfoEdit(id, systemInfo, image);
                return RedirectToAction(nameof(SystemInfoIndex));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        else
        {
            return View(systemInfo);
        }
    }

    // GET: SystemInfo/Delete/5
    public async Task<IActionResult> SystemInfoDelete(int? id)
    {
        try
        {
            var systemInfo = await _adminService.GetSystemInfo(id);
            return View(systemInfo);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: SystemInfo/Delete/5
    [HttpPost, ActionName("SystemInfoDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SystemInfoDeleteConfirmed(int id)
    {
        try 
        {
            await _adminService.SystemInfoDeleteConfirmed(id);
            return RedirectToAction(nameof(SystemInfoIndex));
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
    }


    // GET: Instruments
    public async Task<IActionResult> InstrumentsIndex(int pId=1)
    {
        try
        {
            var instrument = await _adminService.InstrumentsIndex(pId);
            return View(instrument);
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
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
            await _adminService.InstrumentsCreate(instruments);
            return RedirectToAction(nameof(InstrumentsIndex));
        }
        else
        {
            return View(instruments);
        }
    }

    // GET: Instruments/Edit/5
    public async Task<IActionResult> InstrumentsEdit(int? id)
    {
        try
        {
            var instruments = await _adminService.GetInstrument(id);
            return View(instruments);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: Instruments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstrumentsEdit(int id, [Bind("Id,Instrument,IsActive")] Instruments instruments)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _adminService.InstrumentsEdit(id, instruments);
                return RedirectToAction(nameof(InstrumentsIndex));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        else
        {
            return View(instruments);
        }
    }

    // GET: Instruments/Delete/5
    public async Task<IActionResult> InstrumentsDelete(int? id)
    {
        try
        {
            var instruments = await _adminService.GetInstrument(id);
            return View(instruments);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // POST: Instruments/Delete/5
    [HttpPost, ActionName("InstrumentsDelete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstrumentsDeleteConfirmed(int id)
    {
        try
        {
            await _adminService.InstrumentsDeleteConfirmed(id);
            return RedirectToAction(nameof(InstrumentsIndex));
        }
        catch (ProblemException ex)
        {
            return Problem(ex.Message);
        }
    }

    [ActionName("ApprovePost")]
    // TODO pass post PK id not seoLink 
    // TODO check token validation
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePost( string postSeoLink)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _adminService.ApprovePost(postSeoLink, currentUser);
            return RedirectToAction(actionName: Constants.MainPage, controllerName: "Home");
        }
        catch (Exception ex )
        {
            // TO DO 
            // logger.LogError(ex,"Problem happens during approving the post. {ex.Message}");
            throw;
        }
    }
}