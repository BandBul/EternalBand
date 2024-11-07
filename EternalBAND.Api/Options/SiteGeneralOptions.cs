namespace EternalBAND.Api.Options
{
    public class SiteGeneralOptions
    {
        public const string SiteGeneralOptionKey = "SiteGeneralSettings";

        public string SiteName { get; set; }
        public string SiteDomain { get; set; }
        public string SiteLogo { get; set; }
        public string SiteFavicon { get; set; }
        public string SiteSlogan { get; set; }
    }
}
