using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Gaming.Quiz.Library.Dependency;
using System.IO;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Gaming.Quiz.BGServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
               .UseEnvironment(EnvironmentName.Production)//Explicitly setting to Production
               .ConfigureAppConfiguration((hostContext, config) =>
               {
                   var env = hostContext.HostingEnvironment;
                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                   config.SetBasePath(env.ContentRootPath);
                   config.AddEnvironmentVariables(prefix: "ASPNETCORE_");

                   if (args != null)
                   {
                       config.AddCommandLine(args);
                   }
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddOptions();
                   services.AddServices(hostContext.Configuration);
                   services.Configure<Contracts.Configuration.Services>(hostContext.Configuration.GetSection("Services"));

                   //Background Services
                   services.AddHostedService<BackgroundServices.GamedayMapping>();
                   services.AddHostedService<BackgroundServices.PointsCalculation>();
                   services.AddHostedService<BackgroundServices.EODSettlement>();
                   //services.AddHostedService<BackgroundServices.ServiceReminder>();
                   //services.AddHostedService<BackgroundServices.Analytics>();
                   //services.AddHostedService<BackgroundServices.PercentileUpdate>();

                   //CloudWatch logs
                   //ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
                   ILoggerFactory loggerFactory = new LoggerFactory();
                   loggerFactory.UseCloudWatch(hostContext.Configuration);
                   loggerFactory.CreateLogger<Program>();
                   services.AddSingleton(loggerFactory);
               });

            await builder.RunConsoleAsync();
        }
    }
}
