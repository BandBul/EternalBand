namespace EternalBAND.Models;

public class UserProfileControl
{
    public int Id { get; set; }
    public Users Users { get; set; }
    public string UsersId { get; set; }
    public bool Completed { get; set; }
    public DateTime DateTime { get; set; }
        
}