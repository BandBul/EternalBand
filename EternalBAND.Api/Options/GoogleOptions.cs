namespace EternalBAND.Api.Options
{
    public class GoogleOptions
    {
        public const string GoogleOptionsKey = "GoogleSettings";

        public string ClientId { get; set; } = String.Empty;
        public string ClientSecret { get; set; } = String.Empty;
        public string RedirectUrl { get; set; }
    }
}
