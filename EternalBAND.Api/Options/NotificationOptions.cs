namespace EternalBAND.Api.Options
{
    public class NotificationOptions
    {
        public const string NotificationOptionKey = "NotificationSettings";

        public int MaxNotificationCountInPopup { get; set; } = 5;
    }
}
