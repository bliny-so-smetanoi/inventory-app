﻿using InventoryApp.AppStart.Filters;
using InventoryApp.Contracts.Options;
using InventoryApp.DataAccess;
using InventoryApp.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace InventoryApp.AppStart.ConfigureServices
{
    public class ConfigureServicesAppServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {

                options.UseNpgsql(configuration.GetConnectionString("SqlConnection")).UseSnakeCaseNamingConvention();
            }, ServiceLifetime.Scoped);
            /*services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryDb")).UseSnakeCaseNamingConvention();
            });*/
            services.AddSignalR();
            services.AddMemoryCache();
            
            QuestPDF.Settings.License = LicenseType.Community;
            services.Configure<SecretOption>(configuration.GetSection("Secrets"));
            services.Configure<AwsS3Options>(configuration.GetSection("AwsS3"));
            services.Configure<EmailSenderOptions>(configuration.GetSection("EmailAccountCredentials"));
            services.Configure<SmtpClientOptions>(configuration.GetSection("SMTPClient"));

            services.AddScoped<UserAuthenticationService>();
            services.AddScoped<AwsS3FileUploadService>();
            services.AddScoped<ReportGeneratorService>();
            services.AddScoped<UserActionAttribute>();
            services.AddScoped<MailKitEmailSenderService>();

            services.AddAuthentication();
            services.AddAuthorization();
        }
    }
}
