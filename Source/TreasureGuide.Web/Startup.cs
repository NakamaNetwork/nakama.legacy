using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NakamaNetwork.Entities.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using TreasureGuide.Common;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Web.Configurations;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.Donations;
using TreasureGuide.Web.Services.SearchService.Teams;

namespace NakamaNetwork.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<NakamaNetworkContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("NakamaNetworkContext")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<NakamaNetworkContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new ExceptionLoggerAttribute());
                options.Filters.Add(new ThrottlingAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options =>
            {
                options.AddPolicy("NakamaCORS", config => config.AllowAnyOrigin().WithMethods("GET"));
            });
            var defaultJson = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonConvert.DefaultSettings = () => defaultJson;

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            var secretKey = Configuration["Authentication:Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.Subject = jwtAppSettingOptions[nameof(JwtIssuerOptions.Subject)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = TimeSpan.FromDays(3);
            });
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            var builder = services.AddDataProtection();
            var store = Directory.GetCurrentDirectory() + "/keys";
            builder.PersistKeysToFileSystem(new DirectoryInfo(store));
            builder.SetApplicationName("NakamaNetwork");

            services.AddScoped<IAuthExportService, AuthExportService>();
            services.AddScoped<IPreferenceService, PreferenceService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISmsSender, SmsSender>();
            services.AddScoped<IThrottleService, ThrottleService>();
            services.AddScoped<IMetadataService, MetadataService>();
            services.AddScoped<IDonationService, PaypalDonationService>();
            services.AddScoped<TeamSearchService, TeamDbSearchService>();

            services.AddSingleton(x => Configuration);
            services.AddSingleton(x => MapperConfig.Create());

            services.AddAuthentication()
                .AddGoogle(o =>
                {
                    o.ClientId = Configuration["Authentication:Google:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                })
                .AddFacebook(o =>
                {
                    o.ClientId = Configuration["Authentication:Facebook:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
                })
                .AddTwitter(o =>
                {
                    o.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                    o.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                    o.RetrieveUserDetails = true;
                }).AddReddit(o =>
                {
                    o.ClientId = Configuration["Authentication:Reddit:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Reddit:ClientSecret"];
                }).AddTwitch(o =>
                {
                    o.ClientId = Configuration["Authentication:Twitch:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Twitch:ClientSecret"];
                }).AddDiscord(o =>
                {
                    o.ClientId = Configuration["Authentication:Discord:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Discord:ClientSecret"];
                    o.Scope.Add("identity");
                    o.Scope.Add("email");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
