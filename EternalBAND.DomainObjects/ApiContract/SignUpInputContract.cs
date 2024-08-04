namespace EternalBAND.DomainObjects.ApiContract
{
    public class SignUpInputContract : IContract
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
