using DocParty.Models;
using DocParty.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using MediatR;
using System.Text;
using DocParty.Services.Repositories;
using DocParty.Services.Email;
using DocParty.Services;

namespace DocParty
{
    public class Startup
    {
        private const string AuthLocationConfigPath = "Auth:Location";
        private const string AuthGoogleIdConfigPath = "Auth:Google:Id";
        private const string AuthGoogleKeyConfigPath = "Auth:Google:Secret";

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var builder = new StringBuilder();
            string russianSymbols = builder.GetRussianSymbols().ToString();

            services.AddIdentity<User, Role>(options => 
                { 
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = options.User.AllowedUserNameCharacters + russianSymbols;
                })
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    opts.ClientId = Configuration[AuthGoogleIdConfigPath];
                    opts.ClientSecret = Configuration[AuthGoogleKeyConfigPath];
                    opts.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = Configuration[AuthLocationConfigPath];
            });

            services.AddControllersWithViews();
            
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IRepository<byte[],string>,AwsS3FileRepository>();
            services.AddScoped<AuthorAssignService>();
            services.AddScoped<EmailSender>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=account}/{action=login}");
                endpoints.MapControllerRoute("projects", "{userName}/projects");
                endpoints.MapControllerRoute("user", "{userName}");
            });
        }
    }
}
