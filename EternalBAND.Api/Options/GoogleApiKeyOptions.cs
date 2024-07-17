namespace EternalBAND.Api.Options
{
    public class GoogleApiKeyOptions
    {
        public const string GoogleApiKey = "GoogleApiKey";

        public string ClientId { get; set; } = String.Empty;
        public string ClientSecret { get; set; } = String.Empty;
    }
}
