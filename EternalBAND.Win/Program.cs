using EternalBAND.Api.Options;
using EternalBAND.Api.Infrastructure;
using EternalBAND.DataAccess.Infrastructure;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using EternalBAND.Win.Infrastructure;
using NLog;
using NLog.Extensions.Logging;
using EternalBAND.Common;
using System.Diagnostics.Metrics;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy;

var nlogger = LogManager.GetCurrentClassLogger();

var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);
LogManager.Setup().LoadConfigurationFromFile("nlog.config");

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
});
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

try
{
    builder.Services.AddConfiguration(builder.Configuration);
    builder.Services.AddDatabase(builder.Configuration);

    var jwtTokenOptions = new JwtTokenOptions();
    builder.Configuration.GetSection(JwtTokenOptions.JwtOptionKey).Bind(jwtTokenOptions);

    var googleSettings = new GoogleOptions();
    builder.Configuration.GetSection(GoogleOptions.GoogleOptionsKey).Bind(googleSettings);

    builder.Services
        .AddControllersWithViews()
        .AddRazorRuntimeCompilation();
    builder.Services.AddSignalR();

    builder.Services.AddRazorPages(options =>
    {
        // TODO define below magic strings under a class
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Profile", "/profil/{userId?}");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "/sifre-sifirla");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Lockout", "/hesap-kilitlendi");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/kayit-ol");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", $"/{UrlConstants.Login}");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPassword", "/sifremi-unuttum");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", $"/{UrlConstants.AccessDenied}");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPasswordConfirmation", "/mail-gonderildi");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ResendEmailConfirmation", "/yeniden-dogrulama-maili-gonder");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/RegisterConfirmation", "/dogrulama-maili-gonderildi");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ConfirmEmail", $"/{UrlConstants.ConfirmEmail}");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ProfileEdit", $"/{UrlConstants.ProfileEdit}");
    });

    builder.Services
        .AddFluentValidationAutoValidation()
        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
        .AddAuthorizationInternal()
        .AddAuthentication(googleSettings, jwtTokenOptions)
        .AddSwagger()
        .AddDataAccessInfrastructure()
        .AddApiInfrastructure()
        .AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
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
        
    //builder.Services.AddReverseProxy()
    //.LoadFromMemory(
    //    new[]
    //    {
    //        new RouteConfig()
    //        {
    //            RouteId = "react-route",
    //            ClusterId = "react-cluster",
    //            Match = new RouteMatch()
    //            {
    //                Path = "/react/{**catch-all}"   // /react altındaki tüm istekler proxy'lenir
    //            }
    //        }
    //    },
    //    new[]
    //    {
    //        new ClusterConfig()
    //        {
    //            ClusterId = "react-cluster",
    //            Destinations = new Dictionary<string, DestinationConfig>()
    //            {
    //                { "react-destination", new DestinationConfig() { Address = "http://localhost:3000/" } }
    //            }
    //        }
    //    });


    builder
        .Build()
        .UseApplication(builder);
    nlogger.Info("App is starting...");
}

catch(Exception ex)
{
    nlogger.Fatal(ex, "Startup failure..." + ex.Message);
    throw;
}