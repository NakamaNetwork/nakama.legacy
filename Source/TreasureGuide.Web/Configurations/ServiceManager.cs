using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters += " ";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                options.SslPort = 44387;
                options.Filters.Add(new RequireHttpsAttribute());
            }).AddJsonOptions(json =>
            {
                json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddScoped<TreasureEntities>(x => new TreasureEntities(configuration.GetConnectionString("TreasureEntities")));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddSingleton<IMapper>(x => MapperConfig.Create());
        }
    }
}
