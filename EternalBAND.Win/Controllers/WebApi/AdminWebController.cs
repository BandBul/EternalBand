using EternalBAND.Api.Exceptions;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using X.PagedList;

namespace EternalBAND.Win.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminWebController : ControllerBase
    {
        private readonly AdminService _adminService;
        private readonly ControllerHelper _controllerHelper;

        public AdminWebController(AdminService adminService, ControllerHelper controllerHelper)
        {
            _adminService = adminService;
            _controllerHelper = controllerHelper;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> BlogsIndex(int pId = 1)
        {
            try
            {
                var blogs = await _adminService.BlogsIndex(pId);
                return Ok(blogs);
            }
            catch (ProblemException ex)
            {
                return Problem(ex.Message);
            }
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        public async Task<IActionResult> BlogsCreate(
            [FromBody] Blogs blogs,
            List<IFormFile>? uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                await _adminService.BlogsCreate(blogs, uploadedFiles);
                return RedirectToAction(nameof(BlogsIndex));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

            }
            return Ok(blogs);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBlogs(int? id)
        {
            try
            {
                var blog = await _adminService.BlogsEditInitial(id);
                return Ok(blog);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        // TODO please check this method it may not work due to FromForm
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlogsEdit(
            [FromForm] Blogs blogs,
            [FromForm] List<IFormFile>? uploadedFiles,
            [FromForm] List<string>? deletedFilesIndex)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _adminService.BlogsEdit(blogs, uploadedFiles, deletedFilesIndex);
                    return RedirectToAction(nameof(BlogsIndex));
                }
                catch (NotFoundException)
                {
                    return NotFound();
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return Ok(blogs);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> BlogsDeleteConfirmed(int id)
        {
            try
            {
                var result = await _adminService.BlogsDeleteConfirmed(id);
                return Ok(result);
            }
            catch (JsonException ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ContactsIndex(int pId = 1)
        {
            try
            {
                var contacts = await _adminService.ContactsIndex(pId);
                return Ok(contacts);
            }
            catch (ProblemException ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ContactsDetails(int? id)
        {
            try
            {
                var contacts = await _adminService.GetContacts(id);
                return Ok(contacts);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetContact(int? id)
        {
            try
            {
                var contacts = await _adminService.GetContacts(id);
                return Ok(contacts);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactsEdit(int id, [FromBody] Contacts contacts)
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
                return Ok(contacts);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ContactsDelete(int? id)
        {
            try
            {
                var contacts = await _adminService.GetContacts(id);
                return Ok(contacts);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Contacts/Delete/5
        [HttpPost("[action]")]
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
        #region PostTypes

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPostType(int? id)
        {
            try
            {
                var postType = await _adminService.GetPostType(id);
                return Ok(postType);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPostTypes(int pId = 1)
        {
            try
            {
                var postTypes = await _adminService.PostTypesIndex(pId);
                return Ok(postTypes);
            }
            catch (ProblemException ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePostTypes([FromBody] PostTypes postTypes)
        {
            if (ModelState.IsValid)
            {
                await _adminService.PostTypesCreate(postTypes);
                return RedirectToAction(nameof(GetPostTypes));
            }
            else
            {
                return Ok(postTypes);
            }
        }



        // POST: PostTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostTypes(int id, [FromBody] PostTypes postTypes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _adminService.PostTypesEdit(id, postTypes);
                    return RedirectToAction(nameof(GetPostTypes));
                }
                catch (NotFoundException)
                {
                    return NotFound();
                }
            }
            else
            {
                return Ok(postTypes);
            }
        }

        // POST: PostTypes/Delete/5
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostTypes(int id)
        {
            try
            {
                await _adminService.PostTypesDeleteConfirmed(id);
                return Ok();
            }
            catch (ProblemException ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        [HttpGet("[action]")]
        // GET: Instruments
        public async Task<IActionResult> InstrumentsIndex(int pId = 1)
        {
            try
            {
                var instrument = await _adminService.InstrumentsIndex(pId);
                return Ok(instrument);
            }
            catch (ProblemException ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstrumentsCreate([FromBody] Instruments instruments)
        {
            if (ModelState.IsValid)
            {
                await _adminService.InstrumentsCreate(instruments);
                return RedirectToAction(nameof(InstrumentsIndex));
            }
            else
            {
                return Ok(instruments);
            }
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstrumentsEdit(int id, [FromBody] Instruments instruments)
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
                return Ok(instruments);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetInstrument(int? id)
        {
            try
            {
                var instruments = await _adminService.GetInstrument(id);
                return Ok(instruments);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: Instruments/Delete/5
        [HttpPost("[action]")]
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

        [HttpGet("[action]")]
        // TODO pass post PK id not seoLink 
        // TODO check token validation
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePost(string postSeoLink)
        {
            try
            {
                var currentUser = await _controllerHelper.GetUserAsync(User);
                await _adminService.ApprovePost(postSeoLink, currentUser);
                return RedirectToAction(nameof(PostApprovePanelIndex));
            }
            catch (Exception ex)
            {
                // TO DO 
                // logger.LogError(ex,"Problem happens during approving the post. {ex.Message}");
                throw;
            }
        }

        [HttpGet("[action]")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> PostApprovePanelIndex(int pId = 1)
        {
            return Ok(await GetApprovalPageData(pId));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> PostForApproval(string? seolink)
        {
            if (seolink == null)
            {
                // TODO return error like post is not exist or null 
                return RedirectToAction(nameof(EndpointConstants.Anasayfa));
            }
            var model = await _adminService.Post(seolink);
            if (model == null)
            {
                //TODO past this warning message to the dto
                //TempData["WarningMessage"] = $"'{seolink}' id li ilan yayından kaldırılmıştır";
                return RedirectToAction(nameof(PostApprovePanelIndex));
            }

            if (model.Status != PostStatus.PendingApproval)
            {
                //TODO past this warning message to the dto
                //TempData["WarningMessage"] = $"'{seolink}' id li ilan onay aşamasında değildir. Status : {model.Status.ToString()}";
                return RedirectToAction(nameof(PostApprovePanelIndex));
            }

            return Ok(model);
        }

        private async Task<IPagedList<Posts>> GetApprovalPageData(int pId = 1)
        {
            return await _adminService.GetFilteredPosts(s => s.Status == PostStatus.PendingApproval);
        }
    }
}
