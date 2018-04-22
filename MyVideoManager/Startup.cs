using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyVideoManager.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Identity;

namespace MyVideoManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=MyVideoManager;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<WorkContext>(options =>
    options.UseSqlServer(connection));
            services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connection));

            // User InMemoryDatabase for test
    //        services.AddDbContext<UserDbContext>(options =>
    //options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                //options.Password.RequireLowercase = false;
                //options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                // options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                // options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });



            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MVM API", Version = "v0.1" });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            // Get rended result at http://localhost:<random_port>/swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
