using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TreasureGuide.Entities;
using TreasureGuide.Web.Data;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Configurations
{
    public static class ServiceConfig
    {
        public static void Configure(IServiceCollection services, IConfigurationRoot configuration, SecurityKey securityKey)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TreasureEntities")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                    options.User.AllowedUserNameCharacters += " ";
                    options.Cookies.ApplicationCookie.AutomaticChallenge = false;
                    options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api"))
                            {
                                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                ctx.Response.WriteAsync($"{{\"error\": {ctx.Response.StatusCode}}}");
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            return Task.FromResult(0);
                        }
                    };
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new ExceptionLoggerAttribute());
            }).AddJsonOptions(json =>
            {
                json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            var defaultJson = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonConvert.DefaultSettings = () => defaultJson;

            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.Subject = jwtAppSettingOptions[nameof(JwtIssuerOptions.Subject)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            });

            var builder = services.AddDataProtection();
            var store = Directory.GetCurrentDirectory() + "/keys";
            builder.PersistKeysToFileSystem(new DirectoryInfo(store));
            builder.SetApplicationName("NakamaNetwork");

            services.AddScoped(x => new TreasureEntities(configuration.GetConnectionString("TreasureEntities")));
            services.AddScoped<IAuthExportService, AuthExportService>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IThrottleService, ThrottleService>();

            services.AddSingleton(x => MapperConfig.Create());
        }
    }
}
