
using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EternalBAND.Business
{
    // TODO REMOVE
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

        
        public static dynamic GetDynamicJson()
        {
            return  GetDynamicJson(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
        }
        public static dynamic GetDynamicJson(string path)
        {
           
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

        }
        public static string GetAppSettings(dynamic jsonObj,string searchText)
        {
            return jsonObj.SiteGeneralSetting[searchText];
        }



      
    }
}