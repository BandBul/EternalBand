using EternalBAND.Common;

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

    public int? MessageBoxId { get; set; }
    public virtual MessageBox MessageBox { get; set; }
    // "mesajlar/id/postid"
    public string RedirectLink => 
        $"{EndpointConstants.Messages}/{SenderUserId}/{RelatedPostId}";


}