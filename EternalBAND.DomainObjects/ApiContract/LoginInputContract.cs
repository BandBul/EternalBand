namespace EternalBAND.DomainObjects.ApiContract
{
    public class LoginInputContract
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
