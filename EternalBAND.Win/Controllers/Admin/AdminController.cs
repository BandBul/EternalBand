using EternalBAND.Api.Exceptions;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EternalBAND.Controllers.Admin;

[Authorize(Roles = "Admin")]
[ApiExplorerSettings(IgnoreApi = true)]
public class AdminController : Controller
{
    private readonly AdminService _adminService;
    private readonly ControllerHelper _controllerHelper;

    public AdminController(AdminService adminService, ControllerHelper controllerHelper)
    {
        _adminService = adminService;
        _controllerHelper = controllerHelper;
    }
    [HttpGet]
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

    [HttpGet]
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
        [Bind("Id,Title,SummaryText,HtmlText,Tags,SeoLink")]
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
    [HttpGet]
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
        [Bind("Id,Title,SummaryText,HtmlText,Tags,SeoLink")]
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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


    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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


    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet,ActionName("ApprovePost")]
    // TODO pass post PK id not seoLink 
    // TODO check token validation
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePost( string postSeoLink)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _adminService.ApprovePost(postSeoLink, currentUser);
            return RedirectToAction(nameof(PostApprovePanelIndex));
        }
        catch (Exception ex )
        {
            // TO DO 
            // logger.LogError(ex,"Problem happens during approving the post. {ex.Message}");
            throw;
        }
    }

    [HttpGet, ActionName("RejectPost")]
    // TODO pass post PK id not seoLink 
    // TODO check token validation
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectPost(string postSeoLink)
    {
        try
        {
            var currentUser = await _controllerHelper.GetUserAsync(User);
            await _adminService.RejectPost(postSeoLink, currentUser);
            return RedirectToAction(nameof(PostApprovePanelIndex));
        }
        catch (Exception ex)
        {
            // TO DO 
            // logger.LogError(ex,"Problem happens during approving the post. {ex.Message}");
            throw;
        }
    }
    [HttpGet]
    public async Task<IActionResult> PostApprovePanelIndex(int pId=1)
    {
        return View(await GetApprovalPageData(pId));
    }
    [ActionName("PostForApproval")]
    [HttpGet]
    public async Task<IActionResult> PostForApproval(string? seolink)
    {
        if (seolink == null)
        {
            // TODO return error like post is not exist or null 
            return RedirectToAction(nameof(Constants.MainPage));
        }
        ViewBag.ApprovalPurpose = true;
        var model = await _adminService.Post(seolink);
        if (model == null)
        {
            TempData["WarningMessage"] = $"'{seolink}' id li ilan yayýndan kaldýrýlmýþtýr";
            return RedirectToAction(nameof(PostApprovePanelIndex));
        }

        if(model.Status != PostStatus.PendingApproval)
        {
            TempData["WarningMessage"] = $"'{seolink}' id li ilan onay aþamasýnda deðildir. Status : {model.Status.ToString()}";
            return RedirectToAction(nameof(PostApprovePanelIndex));
        }

        return View("~/Views/Home/Post.cshtml", model);
    }

    private async Task<IPagedList<Posts>> GetApprovalPageData(int pId = 1)
    {
        return await _adminService.GetFilteredPosts(s => s.Status == PostStatus.PendingApproval);
    }

}