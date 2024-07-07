using Microsoft.AspNetCore.Authentication;

namespace EternalBAND.DomainObjects.ViewModel
{
    public class ExternalLoginPageModel
    {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        public string ButtonTextSuffix { get; set; }
    }
}
