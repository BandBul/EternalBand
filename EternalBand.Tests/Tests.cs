using EternalBAND.Api;
using EternalBAND.Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EternalBand.Tests
{
    [TestFixture]
    public class Tests
    {
        private IConfiguration configuration;
        private IServiceCollection services;
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private IServiceProvider serviceProvider;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base path to the current directory
                .AddJsonFile("appsettings_test.json", optional: true, reloadOnChange: true) // Add the appsettings.json
                .Build();
            
            services = new ServiceCollection();
            services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SmtpOptionKey));
            services.Configure<SiteGeneralOptions>(configuration.GetSection(SiteGeneralOptions.SiteGeneralOptionKey));

            services.AddScoped<IBaseEmailSender, EmailSender>();
            services.AddLogging(configure => configure.AddConsole());

            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task Test_Send_Email()
        {
            var emailSender = serviceProvider.GetService<IBaseEmailSender>();
            await emailSender.SendEmailAsync("enginyldrm02at@gmail.com", "Bandbul SMTP Test", "SMTP icin deneme maili atiyorum");
            await emailSender.SendEmailAsync("enisemre8st@gmail.com ", "Bandbul SMTP Test", "SMTP icin deneme maili atiyorum");
            await emailSender.SendEmailAsync("brkysgmn@gmail.com", "Bandbul SMTP Test", "SMTP icin deneme maili atiyorum");
        }
    }
}