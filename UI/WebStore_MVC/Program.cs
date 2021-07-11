using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace WebStore_MVC
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging((host, log) => log
                //.ClearProviders()
                //.AddDebug()
                //.AddConsole()
                //.AddEventLog())            
                .ConfigureWebHostDefaults(host => host
                .UseStartup<Startup>()
                )
            .UseSerilog((host, log)=> log.ReadFrom.Configuration(host.Configuration)
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                  .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                  .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
                  //.WriteTo.Seq("http://localhost:5341")
                  )
            ;
    }
}
