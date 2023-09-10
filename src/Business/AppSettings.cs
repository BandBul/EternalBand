
using EternalBAND.Data;
using EternalBAND.Models;

namespace EternalBAND.Business
{
    public class AppSettings
    {
        private static ApplicationDbContext _context;
        private static  IHttpContextAccessor _httpContextAccessor;

        public AppSettings()
        {
            
        }
        public AppSettings(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public AppSettings(ApplicationDbContext context)
        {
            _context = context;
        }
        public static void SetAppSettingValue(string key, string value)
        {
            string appSettingsJsonFilePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            var jsonObj = GetDynamicJson(appSettingsJsonFilePath);
            jsonObj.SiteGeneralSetting[key] = value;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);

        }
        public static void SetSiteDomainValue(string value)
        {
            string appSettingsJsonFilePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            var jsonObj = GetDynamicJson(appSettingsJsonFilePath);
            jsonObj.SiteGeneralSetting["SiteDomain"] = value;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);

        }
        public static dynamic GetDynamicJson()
        {
            return  GetDynamicJson(System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json"));
        }
        public static dynamic GetDynamicJson(string path)
        {
           
            var json = System.IO.File.ReadAllText(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

        }
        public static string GetAppSettings(dynamic jsonObj,string searchText)
        {
            return jsonObj.SiteGeneralSetting[searchText];
        }
        
        public string GetSiteTitle()
        {
            return GetAppSettings(AppSettings.GetDynamicJson(),"SiteName");
        }
        public string GetSiteFavicon()
        {
            return GetAppSettings(AppSettings.GetDynamicJson(),"SiteFavicon");
        }
        public string GetSiteLogo()
        {
            return GetAppSettings(AppSettings.GetDynamicJson(),"SiteLogo");
        }
        public string GetSiteDomain()
        {
            return GetAppSettings(AppSettings.GetDynamicJson(),"SiteDomain");
        }
        public double GetMeetingPrice()
        {
            return Convert.ToDouble(_context.SystemInfo.FirstOrDefault(n=> n.Type == "meeting-price").Value);
        }
        public string GetRandomMailDomain()
        {
            return "@randommailvzVyOxeXMYnNGqDP.com"; // domain değişebilir ihtimaline bakılarak domain name ile yapmadım
        }

        public string MeetingHostToken()
        {
            return
                "&jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb250ZXh0Ijp7InVzZXIiOnsiYXZhdGFyIjoiaHR0cHM6L2dyYXZhdGFyLmNvbS9hdmF0YXIvYWJjMTIzIiwibmFtZSI6IkVnZSBTYcSfbMSxayIsImVtYWlsIjoiamRvZUBleGFtcGxlLmNvbSIsImlkIjoiYWJjZDphMWIyYzMtZDRlNWY2LTBhYmMxLTIzZGUtYWJjZGVmMDFmZWRjYmEifSwiZ3JvdXAiOiJhMTIzLTEyMy00NTYtNzg5In0sImF1ZCI6ImVnZSIsImlzcyI6ImVnZSIsInN1YiI6Im1lZC1zdHJlYW0uZWdlLmVkdS50ciIsInJvb20iOiIqIn0.UyPOAVzjh654fmQsZRNoYwT2EGey5KGZ4szqSvAAcIk";
        }
      
    }
}