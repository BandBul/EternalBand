using EternalBAND.Common;
using Microsoft.EntityFrameworkCore;

namespace EternalBAND.DomainObjects;

public class Messages : IEntity
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
    public int? RelatedPostId { get; set; }
    // "mesajlar/id?postid=postid"
    public string RedirectLink => 
        $"{Constants.MessagesQueryParameter}/{SenderUserId}?{Constants.PostIdQueryParameter}={RelatedPostId}";


}