using JwtAuthenticationHelper.Abstractions;
using JwtAuthenticationHelper.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JwtAuthenticationHelper.Extensions
{
    /// <summary>
    /// Simple extension class to encapsulate data protection and cookie auth boilerplate.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthenticationWithProtectedCookie(
            this IServiceCollection services,
            TokenOptions tokenOptions,
            string applicationDiscriminator = null,
            AuthUrlOptions authUrlOptions = null)
        {
            if (tokenOptions == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(tokenOptions)} is a required parameter. " +
                    $"Please make sure you've provided a valid instance with the appropriate values configured.");
            }

            var hostingEnvironment = services.BuildServiceProvider()
                .GetService<IHostingEnvironment>();

            services.AddDataProtection(options =>
            options.ApplicationDiscriminator =
                $"{applicationDiscriminator ?? hostingEnvironment.ApplicationName}")
                .SetApplicationName(
                $"{applicationDiscriminator ?? hostingEnvironment.ApplicationName}");

            services.AddScoped<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(
                serviceProvider =>
                new JwtTokenGenerator(
                    tokenOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromMinutes(1);

                options.TicketDataFormat = new JwtAuthTicketFormat(
                    tokenOptions.ToTokenValidationParams(),
                    services.BuildServiceProvider()
                        .GetService<IDataSerializer<AuthenticationTicket>>(),
                    services.BuildServiceProvider()
                        .GetDataProtector(new[]
                        {
                            $"{applicationDiscriminator ?? hostingEnvironment.ApplicationName}-Auth1"
                        }));

                options.LoginPath = authUrlOptions != null ?
                    new PathString(authUrlOptions.LoginPath)
                    : new PathString("/Account/Login");
                options.LogoutPath = authUrlOptions != null ?
                    new PathString(authUrlOptions.LogoutPath)
                    : new PathString("/Account/Logout");
                options.AccessDeniedPath = options.LoginPath;
                options.ReturnUrlParameter = authUrlOptions?.ReturnUrlParameter ?? "returnUrl";
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthenticationForAPI(
            this IServiceCollection services,
            TokenOptions tokenOptions)
        {
            if (tokenOptions == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(tokenOptions)} is a required parameter. " +
                    $"Please make sure you've provided a valid instance with the appropriate values configured.");
            }

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(serviceProvider =>
                new JwtTokenGenerator(tokenOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).
            AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters =
                tokenOptions.ToTokenValidationParams();
            });

            return services;
        }
    }
}