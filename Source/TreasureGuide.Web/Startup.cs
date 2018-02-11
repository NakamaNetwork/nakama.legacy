using System;
using System.IO;
using System.Net;
using System.Text;
using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.Reddit;
using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TreasureGuide.Web.Configurations;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;

namespace TreasureGuide.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            var secretKey = Configuration["Authentication:Jwt:Key"];
            SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        public IConfigurationRoot Configuration { get; }
        public SymmetricSecurityKey SigningKey { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceConfig.Configure(services, Configuration, SigningKey);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("logs/nakama-errors-{Date}.txt", LogLevel.Error);
            loggerFactory.AddFile("logs/nakama-{Date}.txt", LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                loggerFactory.AddDebug();
            }
            else
            {
                app.UseStatusCodePages();
            }

            app.UseRewriter(new RewriteOptions()
                .AddRedirectToHttpsPermanent()
                .Add(new RedirectWwwRule()));

            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseIdentity();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });

            app.UseFacebookAuthentication(new FacebookOptions
            {
                ClientId = Configuration["Authentication:Facebook:ClientId"],
                ClientSecret = Configuration["Authentication:Facebook:ClientSecret"]
            });

            app.UseTwitterAuthentication(new TwitterOptions
            {
                ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"],
                ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"],
                RetrieveUserDetails = true
            });

            app.UseRedditAuthentication(new RedditAuthenticationOptions
            {
                ClientId = Configuration["Authentication:Reddit:ClientId"],
                ClientSecret = Configuration["Authentication:Reddit:ClientSecret"]
            });

            app.UseTwitchAuthentication(new TwitchAuthenticationOptions
            {
                ClientId = Configuration["Authentication:Twitch:ClientId"],
                ClientSecret = Configuration["Authentication:Twitch:ClientSecret"]
            });

            app.UseDiscordAuthentication(new DiscordAuthenticationOptions
            {
                ClientId = Configuration["Authentication:Discord:ClientId"],
                ClientSecret = Configuration["Authentication:Discord:ClientSecret"],
                Scope = { "identify", "email" }
            });

            RoleConfig.Configure(roleManager);

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Items[MetaResultModel.StateKey] = context.Request.Path.Value;
                    context.Request.Path = "/";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}