using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddMvc();
            services.AddScoped<TreasureEntities>(x => new TreasureEntities(configuration.GetConnectionString("TreasureEntities")));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddSingleton<IMapper>(x => MapperConfig.Create());
        }
    }
}
