using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Logger;
using WebStore_MVC.Data;
using WebStore_MVC.Services.InCookies;
using WebStore_MVC.Services.InSql;
using WebStore_MVC.Services.Interfaces;

namespace WebStore.API
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
            var database_name = Configuration["Database"];
            switch (database_name)
            {
                case "MSSQL":
                    services.AddDbContext<WebStoreDB>(opt =>
              opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreDB>(opt =>
          opt.UseSqlite(Configuration.GetConnectionString("Sqlite"),
          o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
                    break;
            }
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IEmployeesData, SqlEmployeesData>();
            services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<IOrderService, SqlOrderService>();

            services.AddTransient<WebStoreDBInitializer>();

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<WebStoreDB>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opt.Lockout.MaxFailedAccessAttempts = 10;
            });
            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.LogoutPath = "/Account/Logout";

                opt.SlidingExpiration = true;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory log)
        {
            log.AddLog4Net();
            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
