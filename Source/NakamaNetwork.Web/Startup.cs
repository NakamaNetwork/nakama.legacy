using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NakamaNetwork.Entities.Models;

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
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<NakamaNetworkContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
