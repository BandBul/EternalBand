namespace EternalBAND.Models;

public class UserContacts
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string SenderUserId { get; set; }
    public string Title { get; set; }
    public DateTime DateTime { get; set; }
}