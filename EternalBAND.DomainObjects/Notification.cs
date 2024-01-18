using EternalBAND.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace EternalBAND.DomainObjects;

public class Notification
{
   
    public int Id { get; set; }
    public string Message { get; set; }
    public string RelatedElementId { get; set; }
    public string RedirectLink { get; set; }
    public Users? SenderUser { get; set; }
    public string? SenderUserId { get; set; }
    public Users ReceiveUser { get; set; }
    public string ReceiveUserId { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
    public NotificationType NotificationType{ get; set; }
}