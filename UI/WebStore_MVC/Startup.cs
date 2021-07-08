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
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Identity;
using Microsoft.Extensions.Logging;
using WebStore.Logger;

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
          //  var database_name = Configuration["Database"];
          //  switch (database_name)
          //  {
          //      case "MSSQL":
          //          services.AddDbContext<WebStoreDB>(opt =>
          //    opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
          //          break;
          //      case "Sqlite":
          //          services.AddDbContext<WebStoreDB>(opt =>
          //opt.UseSqlite(Configuration.GetConnectionString("Sqlite"),
          //o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
          //          break;
          //  }


            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllersConvention())).AddRazorRuntimeCompilation();
           // services.AddTransient<WebStoreDBInitializer>();
            services.AddScoped<ICartService, InCookiesCartService>();
            services.AddIdentity<User, Role>()
                .AddIdentityWebStoreWebAPIClients()
                .AddDefaultTokenProviders();

            //  services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))


            //services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))
            //   .AddTypedClient<IUserStore<User>, UsersClient>()
            //   .AddTypedClient<IUserRoleStore<User>, UsersClient>()
            //   .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
            //   .AddTypedClient<IUserEmailStore<User>, UsersClient>()
            //   .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
            //   .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
            //   .AddTypedClient<IUserClaimStore<User>, UsersClient>()
            //   .AddTypedClient<IUserLoginStore<User>, UsersClient>()
            //   .AddTypedClient<IRoleStore<Role>, RolesClient>()
            //    ;
            services.AddIdentityWebStoreWebAPIClients();


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


            services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IEmployeesData, EmployeesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>();



        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();


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
