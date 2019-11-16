using JwtAuthenticationHelper.Types;
using LikeBusLogistic.BLL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JwtAuthenticationHelper.Extensions;

namespace LikeBusLogistic.Web
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
            var tokenOptions = new TokenOptions();
            Configuration.GetSection("TokenOptions").Bind(tokenOptions);

            var authUrlOptions = new AuthUrlOptions();
            Configuration.GetSection("AuthUrlOptions").Bind(authUrlOptions);

            services.AddJwtAuthenticationWithProtectedCookie(tokenOptions, authUrlOptions: authUrlOptions);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiresAdmin",
                    policy =>
                    policy.RequireClaim("HasAdminRights"));
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddScoped(x => new AccountManagementService(connectionString));
            services.AddScoped(x => tokenOptions);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
