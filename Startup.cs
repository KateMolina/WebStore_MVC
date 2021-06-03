using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore_MVC.Infrastructure;
using WebStore_MVC.Infrastructure.Conventions;
using WebStore_MVC.Services;
using WebStore_MVC.Services.Interfaces;

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
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddControllersWithViews(opt=>opt.Conventions.Add(new TestControllersConvention())).AddRazorRuntimeCompilation();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            #region useAuthorization
            // app.UseAuthorization();
            #endregion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greeting", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greeting"]);
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
