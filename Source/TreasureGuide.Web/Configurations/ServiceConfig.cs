﻿using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TreasureGuide.Common;
using TreasureGuide.Entities;
using TreasureGuide.Web.Data;
using TreasureGuide.Common.Helpers;
using TreasureGuide.Common.Models;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;
using TreasureGuide.Web.Services.Donations;
using TreasureGuide.Web.Services.SearchService.Teams;

namespace TreasureGuide.Web.Configurations
{
    public static class ServiceConfig
    {
        public static void Configure(IServiceCollection services, IConfigurationRoot configuration, SecurityKey securityKey)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("TreasureEntities"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(10).EnableRetryOnFailure(0)
                    ));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                    options.User.AllowedUserNameCharacters += " ";

                    options.Cookies.ExternalCookie.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookies.ExternalCookie.SlidingExpiration = true;

                    options.Cookies.ApplicationCookie.AutomaticChallenge = false;
                    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.Cookies.ApplicationCookie.SlidingExpiration = true;
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
                options.Filters.Add(new ThrottlingAttribute());
            }).AddJsonOptions(json =>
            {
                json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                json.SerializerSettings.Converters.Add(new StringTrimmingJsonConverter());
            });
            services.AddCors(options =>
            {
                options.AddPolicy("NakamaCORS", config => config.AllowAnyOrigin().WithMethods("GET"));
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
                options.ValidFor = TimeSpan.FromDays(7);
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

            services.AddScoped(x => new TreasureEntities(configuration.GetConnectionString("TreasureEntities")));
            services.AddScoped<IAuthExportService, AuthExportService>();
            services.AddScoped<IPreferenceService, PreferenceService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISmsSender, SmsSender>();
            services.AddScoped<IThrottleService, ThrottleService>();
            services.AddScoped<IMetadataService, MetadataService>();
            services.AddScoped<IDonationService, PaypalDonationService>();
            services.AddScoped<TeamSearchService, TeamMiniDbSearchService>();

            services.AddSingleton(x => configuration);
            services.AddSingleton(x => MapperConfig.Create());
        }
    }
}
