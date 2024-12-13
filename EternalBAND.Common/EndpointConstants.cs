

namespace EternalBAND.Common
{
    public class EndpointConstants
    {
        #region English endpoint
        // FilterNewPosts is for ajax call in Anasayfa , user will not see in the url
        public const string FilterNewPosts = "FilterNewPosts";
        // SendMessage endpoint is being used by SignalR Hub
        public const string SendMessage = "SendMessage";

        public const string ExternalLogin = "externallogin";
        public const string Googleresponse = "googleresponse";
        public const string NewPosts = "NewPosts";
        public const string SendSupportMessage = "SendSupportMessage";
        public const string Error = "Error";

        public const string PostActivatedAction = "PostActivated";
        public const string PostDeleteAction = "PostDelete";
        public const string AllReadNotificationAction = "AllReadNotification";
        public const string NotificationReadAction = "NotificationRead";
        public const string PostArchivedAction = "PostArchived";
        #endregion

        public const string BlogsEndpoint = "bloglar/{seolink?}";
        public const string Blogs = "bloglar";
        public const string MessagesEndpoint = "mesajlar/{userId?}/{postId?}";
        public const string Messages = "mesajlar";


        public const string PrivacyPolicy = "gizlilikSozlesmesi";
        public const string PostRules = "ilanKurallari";
        public const string About = "hakkimizda";

        public const string Anasayfa = "Anasayfa";
        public const string Blog = "blog/{seoLink}";
        public const string Posts = "ilanlar";
        
        public const string Post = "ilan/{seolink}";
        public const string Notifications = "bildirimler";

        // please check app.js for usage
        public const string PostCreate = "ilanOlusturma";
        public const string PostEdit = "ilanDüzenleme";
        public const string MyPosts = "ilanlarim";


        public const string Contact = "iletisim";
        public const string Kvkk = "KVKK";
        public const string PageNotFound = "hata-olustu/404";
        public const string ErrorRoute = "hata-olustu/{code}";


        


    }
}
