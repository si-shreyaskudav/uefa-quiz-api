using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Gaming.Quiz.Interfaces.Storage;

namespace Gaming.Quiz.Library.Dependency
{
    public static class ApplicationExtension
    {
        public static IApplicationLifetime RegisterRedis(this IApplicationLifetime app, IRedis redis, IOptions<Application> appSettings)
        {
            if (appSettings.Value.Connection.Redis.Apply)
            {
                app.ApplicationStarted.Register(() => { System.Threading.ThreadPool.SetMinThreads(1000, 1000); });
                app.ApplicationStarted.Register(redis.Multiplexer);
                app.ApplicationStopping.Register(redis.Disposer);
            }

            return app;
        }

        public static IHostApplicationLifetime RegisterRedis(this IHostApplicationLifetime app, IRedis redis)
        {
            app.ApplicationStarted.Register(() => { System.Threading.ThreadPool.SetMinThreads(1000, 1000); });
            app.ApplicationStarted.Register(redis.Multiplexer);
            app.ApplicationStopped.Register(redis.Disposer);

            return app;
        }

        public static IApplicationBuilder RegisterSwagger(this IApplicationBuilder app, IHostingEnvironment env, IOptions<Application> appSettings)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //string swaggerConfig = "/config/swagger/Gaming.Quiz.API.json";
            string swaggerConfig = appSettings.Value.CustomSwaggerConfig.BasePath + "/services/config/swagger/Gaming.Quiz.API.json";

            if (env.IsDevelopment())
                swaggerConfig = "/swagger/v1/swagger.json";

            app.UseSwaggerUI(c =>
            {
                //#TOREMEMBER
                //reading swagger json from website's directory location.
                c.SwaggerEndpoint(swaggerConfig, "Gaming.Quiz.API v1");
                c.RoutePrefix = "services";
            });

            return app;
        }


    }

}
