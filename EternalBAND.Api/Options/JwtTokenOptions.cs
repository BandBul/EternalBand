namespace EternalBAND.Api.Options
{
    public class JwtTokenOptions
    {
        public const string JwtOptionKey = "JwtTokenSettings";

        public string Secret { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
    }
}
