using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebStore_MVC
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((host, log) => log
            .ClearProviders()
            .AddDebug()
            .AddConsole()
            .AddEventLog())
            
                .ConfigureWebHostDefaults(host => host
                .UseStartup<Startup>()
                );
    }
}
