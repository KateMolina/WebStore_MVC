using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WebStore.WebAPI.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.TestAPI;
using WebStore_MVC.Data;
using WebStore_MVC.Infrastructure;
using WebStore_MVC.Infrastructure.Conventions;
using WebStore_MVC.Services;
using WebStore_MVC.Services.InCookies;
using WebStore_MVC.Services.InSql;
using WebStore_MVC.Services.Interfaces;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Products;

namespace WebStore_MVC
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


            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllersConvention())).AddRazorRuntimeCompilation();
            services.AddTransient<WebStoreDBInitializer>();
            //services.AddScoped<IProductData, SqlProductData>();
            //services.AddScoped<IEmployeesData, SqlEmployeesData>();
            services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<IOrderService, SqlOrderService>();
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

            services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new Uri(Configuration["WebAPI"]));
            services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new Uri(Configuration["WebAPI"]));
            services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new Uri(Configuration["WebAPI"]));

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize();
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region else
            /* else
             {
                 app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               app.UseHsts();
             }*/
            #endregion
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            #region MiddleWare options
            //app.UseMiddleware<TestMiddleWare>();
            //app.Use(async (context, next) => { await next(); });
            //app.Map("/TestMapRequest", opt => opt.Run(async context =>
            //{ await Task.Delay(100);
            //    var streamWriter = new StreamWriter(context.Response.Body);
            //    await streamWriter.WriteAsync("Hello from TestMapRequest");
            //}
            //)) ;
            #endregion

            app.UseWelcomePage("/WelcomePage");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greeting", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greeting"]);
                });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                 );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
