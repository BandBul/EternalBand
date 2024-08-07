namespace EternalBAND.DomainObjects.ApiContract
{
    public class LoginInputContract : IContract
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
