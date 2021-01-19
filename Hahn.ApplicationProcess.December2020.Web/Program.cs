using Hahn.ApplicationProcess.December2020.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.IO;

namespace Hahn.ApplicationProcess.December2020.Web
{
    public class Program
    {



        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();
         var host = CreateHostBuilder(args).ConfigureLogging(logging => {
                logging.AddFilter("Microsoft", LogLevel.Information);
                logging.AddFilter("System", LogLevel.Error);                   
            }).Build();


            Log.Logger = new LoggerConfiguration().WriteTo.File(config["Serilog:path"]).CreateLogger();

            Log.Information("APP STARTED");

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicantDBContextClass>();

                DataGeneratorClass.Initialize(services);

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
