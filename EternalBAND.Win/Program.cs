using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EternalBAND.DataAccess;
using EternalBAND.Api.Hubs;
using EternalBAND.DomainObjects;
using EternalBAND.Api.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using EternalBAND.Api.Infrastructure;
using EternalBAND.DataAccess.Infrastructure;
using System.Security.Authentication;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using System.Reflection;

var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);
builder.Host.UseWindowsService();
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "BandBul.Api";
});
builder.WebHost.UseKestrel((context, serverOptions) =>
{
    serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
    .Endpoint("HTTPS", listenOptions =>
    {
        listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
    });
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EternalBAND.Migrations")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Swagger Documentation Section
var info = new OpenApiInfo()
{
    Title = "Bandbul Documentation",
    Version = "v1",
    Description = "Herkes icin Muzik",
    //Contact = new OpenApiContact()
    //{
    //    Name = "Your name",
    //    Email = "your@email.com",
    //}

};

builder.Services.AddSwaggerGen(c =>
{ 
    c.SwaggerDoc("v1", info);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddIdentity<Users, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "/profil/{userId?}");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "/sifre-sifirla");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Lockout", "/hesap-kilitlendi");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/kayit-ol");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/giris-yap");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPassword", "/sifremi-unuttum");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "/cikis-yap");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", "/erisim-engellendi");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPasswordConfirmation",
        "/mail-gonderildi"); 
});



var googleApiKey = new GoogleApiKeyOptions();
builder.Configuration.GetSection(GoogleApiKeyOptions.GoogleApiKey).Bind(googleApiKey);
builder.Services.Configure<NotificationOptions>(
    builder.Configuration.GetSection(NotificationOptions.NotificationOptionKey));
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddGoogle(opt =>
{
    opt.ClientId = googleApiKey.ClientId;
    opt.ClientSecret = googleApiKey.ClientSecret;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/giris-yap";
    options.AccessDeniedPath = "/erisim-engellendi";
    options.LogoutPath = "/cikis-yap";
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
});
builder.Services.AddSignalR();

builder.Services
    .AddDataAccessInfrastructure()
    .AddApiInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(u =>
    {
        u.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Bandbul");
    });
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/hata-olustu/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Css")),
    RequestPath = "/Css"
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Anasayfa}/{id?}");
app.MapControllers();
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");


app.Run();





