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
using X.PagedList;

namespace EternalBAND.Controllers.User;

[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    
    private readonly UserManager<Users> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IWebHostEnvironment _environment;

    public UserController(ApplicationDbContext context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment environment)
    {
        _context = context;
        _userManager = userManager;
        _environment = environment;
        _roleManager = roleManager;
    }

    // GET: Posts
    [Route("ilanlarim")]
    public async Task<IActionResult> PostIndex()
    {
        var getUser = await _userManager.GetUserAsync(User);
    
        var applicationDbContext = _context.Posts.Where(n=> n.AddedByUserId==getUser.Id).Include(p => p.AddedByUser).Include(p => p.AdminConfirmationUser)
            .Include(p => p.PostTypes).Include(p => p.Instruments);
        return View(await applicationDbContext.ToListAsync());
    }

    [Route("ilan-olustur")]
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
    [Route("ilan-olustur")]
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
                await _context.Notification.AddAsync(new Notification()
                {
                    IsRead = false,
                    AddedDate = DateTime.Now,
                    Message = $"{currentUser?.Name} '{posts.SeoLink}' id li yeni bir ilan paylaştı.",
                    ReceiveUserId = adminUsers.ElementAt(0).Id,
                    RedirectLink = $"ilan?s={posts.SeoLink}&approvalPurpose=true",
                    RelatedElementId = posts.SeoLink
                });
                posts.Status = Common.PostStatus.PendingApproval;
            }
            else
            {
                posts.Status = Common.PostStatus.Active;
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
    public async Task<IActionResult> PostEdit(Guid id,
        [Bind(
            "Guid,Title,HTMLText,PostTypesId,InstrumentsId,CityId")]
        Posts posts,
        List<IFormFile>? images)
    {
        if (id != posts.Guid)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                PostAddPhoto(posts, images);
                var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(n => n.Guid == posts.Guid);
                var getUser = await _userManager.GetUserAsync(User);
                if ((getUser.Id == post.AddedByUserId))
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
    [Route("mesajlar/{u?}")]
    public async Task<ActionResult> ChatIndex(string? u)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var chats = _context.Messages.Include(n=> n.ReceiverUser).Include(n=> n.SenderUser).Where(n => n.SenderUserId == user.Id || n.ReceiverUserId == user.Id).ToList();
            ChatViewModel viewModel = new(){AllChat = chats};
            ViewBag.UserId = u;
            ViewBag.LoginUserId = user.Id;
            if (u != null) // mesaja tıklandıysa göster
            {
                var chat = _context.Messages.Include(n=> n.ReceiverUser).Include(n=> n.SenderUser).Where(n => (n.ReceiverUserId == u||n.ReceiverUserId == user.Id) && (n.SenderUserId == user.Id || n.SenderUserId ==u)).ToList();
                foreach (var isread in chat.Where(n=> !n.IsRead).ToList())
                {
                    isread.IsRead = true;
                  await  _context.SaveChangesAsync();
                }
                viewModel.Chat = chat;
                return View(viewModel);
            }
            else // mesaj için gelmediyse sağ boş sol mesajlar göster
            {
               
                return View(viewModel);
            }
        }
        else
        {
            return Redirect("/giris-yap");
        }
    }
    [HttpPost, ActionName("SendMessage")]
    public async Task<JsonResult> SendMessage(Guid id,string message)
    {
        if (!_context.Users.Any(n=> n.Id ==id.ToString()))
        {
            return Json("Kayıt bulunamadı.");
        }

        var getUser = await _userManager.GetUserAsync(User);

        try
        {
            await _context.Messages.AddAsync(new Messages()
            {
                SenderUserId = getUser.Id,
                IsRead = false,
                Date = DateTime.Now,
                Message = message,
                MessageGuid = Guid.NewGuid(),
                ReceiverUserId = id.ToString(),
            });
           await new Business.NotificationProcess(_context).SaveNotification($"{getUser.Name} sana bir mesaj gönderdi.", id.ToString(),
                "mesajlar/" + getUser.Id);
            
            await _context.SaveChangesAsync();
            return Json("Mesaj gönderildi.");
        }
        catch(Exception ex)
        {
            return Json("Mesaj gönderilemedi.");
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
       
        return View( await _context.Notification.Where(n => n.ReceiveUserId == getUser.Id).OrderByDescending(n=> n.IsRead).ThenByDescending(n=> n.AddedDate).ToPagedListAsync(pId, 10));
    }
}