using System.ComponentModel;
using EternalBAND.Common;
using Microsoft.AspNetCore.Identity;

namespace EternalBAND.Models;

public class BaseNotificationModel
{
    [DisplayName("NotificationType")]
    public NotificationType? NotificationType { get; set; }
    [DisplayName("Title")]
    public string? Title { get; set; }
    [DisplayName("IconHtmlContext")]
    public string? IconHtmlContext { get; set; }
    [DisplayName("NotificationCountIdName")]
    public string? NotificationCountIdName { get; set; }
    [DisplayName("SignalRBroadCastingTitle")]
    public string? SignalRBroadCastingTitle { get; set; }
}