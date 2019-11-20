using JwtAuthenticationHelper.Types;
using LikeBusLogistic.BLL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JwtAuthenticationHelper.Extensions;
using LikeBusLogistic.BLL;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using LikeBusLogistic.BLL.Variables;
using System.Security.Claims;

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
            var tokenOptions = new JwtAuthenticationHelper.Types.TokenOptions();
            Configuration.GetSection("TokenOptions").Bind(tokenOptions);

            var authUrlOptions = new AuthUrlOptions();
            Configuration.GetSection("AuthUrlOptions").Bind(authUrlOptions);

            services.AddJwtAuthenticationWithProtectedCookie(tokenOptions, authUrlOptions: authUrlOptions);
            services.AddAuthorization();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped(x => new ServiceFactory(connectionString));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
