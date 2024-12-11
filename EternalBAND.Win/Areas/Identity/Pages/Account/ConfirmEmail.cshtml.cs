#nullable disable

using System.Text;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<Users> _userManager;

        public ConfirmEmailModel(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"'{userId}' ID'sine sahip kullanıcı yüklenemedi.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            // TODO please integrate email confirmation mail link here
            StatusMessage = result.Succeeded ? "Email adresinizi onayladığınız için teşekkür ederiz." : "E-posta adresinizi onaylama hatası. Bu bağlantıdan tekrar onaylama e-postası alabilirsiniz.";
            return Page();
        }
    }
}
