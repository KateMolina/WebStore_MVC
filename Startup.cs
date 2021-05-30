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

namespace WebStore_MVC
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
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //  else
            // {
            //   app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //  app.UseHsts();
            //}
           // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TestMiddleWare>();
            app.Use(async (context, next) => { await next(); });
            app.Map("/TestMapRequest", opt => opt.Run(async context =>
            { await Task.Delay(100);
                var streamWriter = new StreamWriter(context.Response.Body);
                await streamWriter.WriteAsync("Hello from TestMapRequest");
            }
            )) ;

            app.UseWelcomePage("/WelcomePage");
           // app.UseAuthorization();

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
