using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EternalBAND.Data;
using EternalBAND.Hubs;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Emit;
using EternalBAND.Business.Options;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using EternalBAND.Business;
using EternalBAND.DomainObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
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
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "/profil");
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

builder.Services.AddScoped<IEmailSender, MailSender>();
builder.Services.AddScoped<MessageService>();

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
