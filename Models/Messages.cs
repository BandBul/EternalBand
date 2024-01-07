using EternalBAND.Common;

namespace EternalBAND.Models;

public class Messages
{
    public int Id { get; set; }
    public Users? SenderUser { get; set; }
    public string? SenderUserId { get; set; }
    public Users? ReceiverUser { get; set; }
    public string? ReceiverUserId { get; set; }
    public DateTime Date { get; set; }
    public string? Message { get; set; }
    public bool IsRead { get; set; }
    public Guid MessageGuid { get; set; }
    public Posts? RelatedPost { get; set; }
    public int RelatedPostId { get; set; }
    // "mesajlar/id?postid=postid"
    public string RedirectLink => 
        $"{Constants.MessagesQueryParameter}/{SenderUserId}?{Constants.PostIdQueryParameter}={RelatedPostId}";


}