using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EternalBAND.DataAccess;
using EternalBAND.Api.Hubs;
using EternalBAND.DomainObjects;
using EternalBAND.Api.Options;
using Microsoft.Extensions.FileProviders;
using EternalBAND.Api.Infrastructure;
using EternalBAND.DataAccess.Infrastructure;
using System.Security.Authentication;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using EternalBAND.Win.Infrastructure;
using NLog;
using NLog.Extensions.Logging;

var nlogger = LogManager.GetCurrentClassLogger();

var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);
LogManager.Setup().LoadConfigurationFromFile("nlog.config");
// Add NLog to the services
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
});
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();


try
{
    var debugOption = new DebugOptions();
    builder.Configuration.GetSection(DebugOptions.DebugOptionKey).Bind(debugOption);
    if (debugOption.IsWebApiEnabled)
    {
        var jwtTokenOptions = new JwtTokenOptions();
        builder.Configuration.GetSection(JwtTokenOptions.JwtOptionKey).Bind(jwtTokenOptions);

        var googleSettings = new GoogleOptions();
        builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey).Bind(googleSettings);

        builder.Services.Configure<JwtTokenOptions>(builder.Configuration.GetSection(JwtTokenOptions.JwtOptionKey));
        builder.Services.Configure<NotificationOptions>(builder.Configuration.GetSection(NotificationOptions.NotificationOptionKey));
        builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey));

        // TODO below settings for able to setup an windows service, research is there any other wy todo
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
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EternalBAND.Migrations")));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Fluent Validation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddControllers();
        builder.Services.AddSignalR();

        builder.Services
            .AddAuthorizationInternal()
            .AddAuthentication(googleSettings, jwtTokenOptions)
            .AddSwagger()
            .AddDataAccessInfrastructure()
            .AddApiInfrastructure();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder
            .Build()
            .UseApplication(builder.Environment.ContentRootPath);
    }

    else
    {
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

        var googleApiKey = new GoogleOptions();
        builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey).Bind(googleApiKey);
        builder.Services.Configure<NotificationOptions>(
            builder.Configuration.GetSection(NotificationOptions.NotificationOptionKey));
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = "Cookies";
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/giris-yap";
            options.AccessDeniedPath = "/erisim-engellendi";
            options.LogoutPath = "/cikis-yap";
            options.ExpireTimeSpan = TimeSpan.FromHours(3);
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
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
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
        });

        builder.Services.AddSignalR();

        builder.Services
            .AddDataAccessInfrastructure()
            .AddApiInfrastructure();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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
        app.UseCors("AllowAnyOrigin");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Anasayfa}/{id?}");
        app.MapControllers();
        app.MapRazorPages();
        app.MapHub<ChatHub>("/chatHub");

        nlogger.Info("App is starting...");
        app.Run();
    }
}

catch(Exception ex)
{
    nlogger.Fatal(ex, "Startup failure..." + ex.Message);
    throw;
}








