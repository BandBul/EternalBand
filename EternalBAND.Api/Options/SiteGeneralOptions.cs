namespace EternalBAND.Api.Options
{
    public class SiteGeneralOptions
    {
        public const string SiteGeneralOptionKey = "SiteGeneralSettings";

        public string Name { get; set; }
        public string Domain { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }
        public string Slogan { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string MetaKeywords { get; set; }
        public string BottomFooterText { get; set; }
    }
}
