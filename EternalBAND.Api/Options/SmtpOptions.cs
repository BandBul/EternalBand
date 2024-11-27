namespace EternalBAND.Api.Options
{
    public class SmtpOptions
    {
        public const string SmtpOptionKey = "SmtpSettings";

        public string SenderAddress { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int Port { get; set; }
        public int TimeoutMs { get; set; }
        public bool SslEnabled { get; set; }
    }
}
