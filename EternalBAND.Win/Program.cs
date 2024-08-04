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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EternalBAND.Win.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;

var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

var debugOption = new DebugOptions();
builder.Configuration.GetSection(DebugOptions.DebugOptionKey).Bind(debugOption);

if (debugOption.IsWebApiEnabled)
{
    var jwtTokenOptions = new JwtTokenOptions();
    builder.Configuration.GetSection(JwtTokenOptions.JwtOptionKey).Bind(jwtTokenOptions);

    builder.Services.Configure<JwtTokenOptions>(
    builder.Configuration.GetSection(JwtTokenOptions.JwtOptionKey));

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


    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    builder.Services.AddControllers();

    //Swagger Documentation Section
    var info = new OpenApiInfo()
        {
            Title = "Bandbul Documentation",
            Version = "v1",
            Description = "Herkes icin Muzik",
        };

    var securityId = "Bearer";
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", info);

        c.AddSecurityDefinition(securityId, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = securityId,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = securityId
                    }
                },
                new string[] {}
            }
        });
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

    var googleSettings = new GoogleOptions();
    builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey).Bind(googleSettings);

    builder.Services.Configure<NotificationOptions>(
        builder.Configuration.GetSection(NotificationOptions.NotificationOptionKey));

    builder.Services.Configure<GoogleOptions>(
    builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenOptions.Secret)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtTokenOptions.Issuer,
            ValidAudience = jwtTokenOptions.Audience,
        };
    })
    .AddGoogle(opt =>
    {
        opt.ClientId = googleSettings.ClientId;
        opt.ClientSecret = googleSettings.ClientSecret;
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
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
    
    app.UseMiddleware<ErrorHandlingMiddleware>();
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
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
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

    app.Run();
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



    var googleApiKey = new GoogleOptions();
    builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey).Bind(googleApiKey);
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


    app.Run();
}







