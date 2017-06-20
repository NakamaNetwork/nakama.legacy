using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using TreasureGuide.Entities;
using TreasureGuide.Web.Data;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Configurations
{
    public static class ServiceManager
    {
        public static void RegisterServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TreasureEntities")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddJsonOptions(json =>
            {
                json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddScoped<TreasureEntities>(x => new TreasureEntities(configuration.GetConnectionString("TreasureEntities")));

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IThrottleService, ThrottleService>();

            services.AddSingleton<IMapper>(x => MapperConfig.Create());
            services.AddMemoryCache();
        }
    }
}
