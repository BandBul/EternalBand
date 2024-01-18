using EternalBAND.Business;
using EternalBAND.Common;
using EternalBAND.Data;
using EternalBAND.Helpers;
using EternalBAND.Models;
using EternalBAND.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using X.PagedList;

namespace EternalBAND.Controllers.User;
// TODO create a Reposıtory for manage _context, controller should only responsible for API and basic validations 
[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Users> _userManager;
    private readonly IWebHostEnvironment _environment;

    public UserController(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment)
    {
        _context = context;
        _userManager = userManager;
        _environment = environment;
    }

    // GET: Posts
    [Route("ilanlarim")]
    public async Task<IActionResult> PostIndex()
    {
        var getUser = await _userManager.GetUserAsync(User);
        ViewBag.PostStatusViewModel = new Dictionary<PostStatus, PostStatusViewModel>() 
        {
            { PostStatus.Active, new PostStatusViewModel()
                {
                    DisplayText = "Aktif",
                    Color = "green",
                    HeaderDisplayText = "Aktif"
                }
            },
            { PostStatus.PendingApproval, new PostStatusViewModel()
                {
                    DisplayText = "Onay Bekliyor",
                    Color = "orange",
                    HeaderDisplayText = "Onay Bekleyen"
                }
            },
            { PostStatus.DeActive, new PostStatusViewModel()
                {
                    DisplayText = "Arşivde",
                    Color = "purple",
                    HeaderDisplayText = "Arşivlenen"
                }
            }
        };

        var applicationDbContext = _context.Posts.Where(n=> n.AddedByUserId==getUser.Id).Include(p => p.AddedByUser).Include(p => p.AdminConfirmationUser)
            .Include(p => p.PostTypes).Include(p => p.Instruments);
        return View(await applicationDbContext.ToListAsync());
    }

    [Route("PostCreate")]
    // GET: Posts/Create
    public IActionResult PostCreate()
    {
        ViewData["PostTypesId"] = new SelectList(_context.PostTypes, "Id", "Type");
        ViewData["InstrumentsId"] = new SelectList(_context.Instruments, "Id", "Instrument");
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
        //TODO: LOGIN DEGILSE REDIRECT
        if (ModelState.IsValid)
        {
            PostAddPhoto(posts, images);
            PostAddSeoLink(posts);
            var currentUser = await _userManager.GetUserAsync(User);
            posts.AddedByUserId = currentUser?.Id;
            posts.AddedDate = DateTime.Now;
            posts.Guid = Guid.NewGuid();
            var isUserAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
            if(!isUserAdmin)
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
                // Engin-TODO need to send all admins
                // Engin-TODO pass SignalR hub and use ReceiveNotification broadcasting title to send message to front end
                await _context.Notification.AddAsync(new Notification()
                {
                    IsRead = false,
                    AddedDate = DateTime.Now,
                    Message = $"{currentUser?.Name} '{posts.SeoLink}' id li yeni bir ilan paylaştı.",
                    ReceiveUserId = adminUsers.ElementAt(0).Id,
                    SenderUserId = currentUser.Id,
                    RedirectLink = $"ilan?s={posts.SeoLink}&approvalPurpose=true",
                    RelatedElementId = posts.SeoLink,
                    NotificationType = NotificationType.PostSharing
                });
                posts.Status = PostStatus.PendingApproval;
            }
            else
            {
                posts.Status = PostStatus.Active;
            }
            _context.Add(posts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PostIndex));
        }
        else
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
        }

        ViewData["PostTypesId"] = new SelectList(_context.PostTypes, "Id", "Type", posts.PostTypesId);
        ViewData["InstrumentsId"] = new SelectList(_context.Instruments, "Id", "Instrument", posts.InstrumentsId);
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", posts.CityId);
        return View(posts);
    }

    // GET: Posts/Edit/5
    [Route("ilan-duzenle")]
    public async Task<IActionResult> PostEdit(Guid? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var posts = await _context.Posts.FirstOrDefaultAsync(n => n.Guid == id);
        if (posts == null)
        {
            return NotFound();
        }

        ViewData["PostTypesId"] = new SelectList(_context.PostTypes, "Id", "Type", posts.PostTypesId);
        ViewData["InstrumentsId"] = new SelectList(_context.Instruments, "Id", "Instrument", posts.InstrumentsId);
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", posts.CityId);
        return View(posts);
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
        if (posts.Guid == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                PostAddPhoto(posts, images);
                var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(n => n.Guid == posts.Guid);
                var currentUser = await _userManager.GetUserAsync(User);
                if ((currentUser.Id == post.AddedByUserId))
                {
                    if (images.Count == 0)
                    {
                        posts.Photo1 = post.Photo1;
                        posts.Photo2 = post.Photo2;
                        posts.Photo3 = post.Photo3;
                        posts.Photo4 = post.Photo4;
                        posts.Photo5 = post.Photo5;
                    }

                    posts.Id = post.Id;
                    posts.AddedByUserId = post.AddedByUserId;
                    posts.AdminConfirmationUserId = post.AdminConfirmationUserId;
                    posts.AdminConfirmation = post.AdminConfirmation;
                    posts.AddedDate = post.AddedDate;
                    posts.SeoLink = post.SeoLink;
                    posts.Guid = post.Guid;
                    posts.Status = PostStatus.PendingApproval;
                    var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
                    await _context.Notification.AddAsync(new Notification()
                    {
                        IsRead = false,
                        AddedDate = DateTime.Now,
                        Message = $"{currentUser?.Name} '{posts.SeoLink}' ilanında güncelleme yaptı",
                        ReceiveUserId = adminUsers.ElementAt(0).Id,
                        SenderUserId = currentUser.Id,
                        RedirectLink = $"ilan?s={posts.SeoLink}&approvalPurpose=true",
                        RelatedElementId = posts.SeoLink,
                        NotificationType = NotificationType.PostSharing
                    });
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostsExists(posts.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(PostIndex));
        }

        ViewData["PostTypesId"] = new SelectList(_context.PostTypes, "Id", "Type", posts.PostTypesId);
        ViewData["InstrumentsId"] = new SelectList(_context.Instruments, "Id", "Instrument", posts.InstrumentsId);
        ViewData["CityId"] = new SelectList(Cities.GetCities(), "Key", "Value", posts.CityId);
        return View(posts);
    }


    //[ValidateAntiForgeryToken]
    [ActionName("PostArchived")]
    public async Task<IActionResult> PostArchived(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var post = _context.Posts.Find(id);
        if(post == null)
        {
            return BadRequest($"There is no post with  this id : {id}");
        }

        post.Status = PostStatus.DeActive;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync();

        return Redirect("/ilanlarim");
    }

    //[ValidateAntiForgeryToken]
    [ActionName("PostActivated")]
    public async Task<IActionResult> PostActivated(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var isAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
        var post = _context.Posts.Find(id);

        if (post == null)
        {
            return BadRequest($"There is no post with  this id : {id}");
        }

        post.Status = isAdmin ? PostStatus.Active : PostStatus.PendingApproval;
        _context.Posts.Update(post);
        if(!isAdmin)
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
            await _context.Notification.AddAsync(new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                Message = $"{currentUser?.Name} '{post.SeoLink}' ilanını arşivden yayına almak istiyor",
                ReceiveUserId = adminUsers.ElementAt(0).Id,
                SenderUserId = currentUser.Id,
                RedirectLink = $"ilan?s={post.SeoLink}&approvalPurpose=true",
                RelatedElementId = post.SeoLink,
                NotificationType = NotificationType.PostSharing
            });
        }

        await _context.SaveChangesAsync();

        return Redirect("/ilanlarim");
    }



    [HttpPost, ActionName("PostDelete")]
    public async Task<JsonResult> DeleteConfirmed(Guid id)
    {
        if (_context.Posts == null)
        {
            return Json("Kayıt bulunamadı.");
        }

        var getUser = await _userManager.GetUserAsync(User);

        var posts = await _context.Posts.FirstOrDefaultAsync(n => n.Guid == id);
        if ((getUser.Id != posts.AddedByUserId) || getUser.Id == null)
        {
            return Json("Bu ilan size ait değil.");
        }
        else
        {
            if (posts != null)
            {
                _context.Posts.Remove(posts);
            }

            await _context.SaveChangesAsync();
            return Json("İlan silindi.");
        }
    }

    private bool PostsExists(int id)
    {
        return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    async void PostAddSeoLink(Posts post)
    {
        string seoLink = post.Title.Replace(" ", "-").ToLower();
        seoLink += new Random().Next(0, 9999999) + new Random().Next(0, 9999);
        while (true)
        {
            if (!_context.Posts.Any(n => n.SeoLink == seoLink))
            {
                post.SeoLink = seoLink;
                break;
            }
        }
    }

    async void PostAddPhoto(Posts posts, List<IFormFile>? images)
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
                    if (posts.Photo1 == null)
                    {
                        posts.Photo1 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (posts.Photo2 == null)
                    {
                        posts.Photo2 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (posts.Photo3 == null)
                    {
                        posts.Photo3 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (posts.Photo4 == null)
                    {
                        posts.Photo4 = "/images/ilan/" + photoName;
                        continue;
                    }

                    if (posts.Photo5 == null)
                    {
                        posts.Photo5 = "/images/ilan/" + photoName;
                        continue;
                    }
                }
            }
        }
    }

    [HttpPost, ActionName("AllReadNotification")]
    public async Task<JsonResult> AllReadNotification()
    {
      

        var getUser = await _userManager.GetUserAsync(User);

        try
        {
            var okunmamisBildirimler = await _context.Notification
                .Where(n => n.ReceiveUserId == getUser.Id && !n.IsRead).ToListAsync();
            foreach (var noti in okunmamisBildirimler)
            {
                noti.IsRead = true;
              
            }
            await _context.SaveChangesAsync();
            return Json("İşlem başarılı.");
        }
        catch(Exception ex)
        {
            return Json("İşlem başarısız.");
        }
    
    }
    [Route("bildirimler")]
    public async Task<IActionResult> NotificationIndex(int pId=1)
    {
        var getUser = await _userManager.GetUserAsync(User);
       
        return View( await _context.Notification.Where(n => n.ReceiveUserId == getUser.Id && n.NotificationType == NotificationType.PostSharing).OrderByDescending(n=> n.IsRead).ThenByDescending(n=> n.AddedDate).ToPagedListAsync(pId, 10));
    }

    [ActionName("NotificationRead")]
    public async Task<IActionResult> NotificationRead(int id)
    {
        var notif  = _context.Notification.Find(id);
        if (notif == null)
        {
            return BadRequest($"There is no notificaiton with this id : {id}");
        }
        notif.IsRead = true;
        _context.Notification.Update(notif);
        await _context.SaveChangesAsync();

        return Redirect("/"+notif.RedirectLink);
    }


}