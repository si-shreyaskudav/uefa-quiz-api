using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching;
using Gaming.Quiz.Contracts.Gameplay;
using Gaming.Quiz.Interfaces.Gameplay;
using Gaming.Quiz.Interfaces.Feeds;

namespace  Gaming.Quiz.Library.Dependency
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<Application>(configuration.GetSection("Application"));
            services.AddSingleton<Interfaces.Storage.IAWS, AWS.SES.Email>();
            services.AddSingleton<Interfaces.Storage.IPostgre, Postgre.Connection>();
            services.AddSingleton<Interfaces.Storage.IRedis, Redis.Action.Broker>();
            services.AddSingleton<Interfaces.Session.ICookies, Session.Cookies>();
            services.AddSingleton<Interfaces.Asset.IAsset, Asset.Constants>();

            return services;
        }

        public static ILoggerFactory UseCloudWatch(this ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            AWSLoggerConfigSection section = configuration.GetAWSLoggingConfigSection();
            loggerFactory.AddAWSProvider(section);
            return loggerFactory;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Gaming.Quiz.API",
                    Version = "V1",
                    Description = "The API documentation for Gaming Quiz APIs"
                });

                //c.OperationFilter<Swashbuckle.AspNetCore.Examples.ExamplesOperationFilter>();
                // Set the comments path for the Swagger JSON and UI.
               
                var xmlFile = "Gaming.Quiz.API.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DocumentFilter<SwaggerDocumentFilter>();
            });

            return services;
        }
    }
}
