using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Library.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gaming.Quiz.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //User-defined extension method
            services.AddServices(Configuration);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime,
            Interfaces.Storage.IRedis redis, Interfaces.Storage.IPostgre postgre, IOptions<Application> appSettings)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            appLifetime.RegisterRedis(redis, appSettings);
            app.UseCors(options => options.WithOrigins("http://localhost:3001").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                name: "rules",
                pattern: "/rules",
                defaults: new { controller = "Home", action = "Rules" });

                endpoints.MapControllerRoute(
                name: "leaderboard",
                pattern: "/leaderboard",
                defaults: new { controller = "Home", action = "Leaderboard" });

                endpoints.MapControllerRoute(
                name: "terms",
                pattern: "/terms",
                defaults: new { controller = "Home", action = "Terms" });

                endpoints.MapControllerRoute(
               name: "disclaimer",
               pattern: "/disclaimer",
               defaults: new { controller = "Home", action = "Disclaimer" });

                endpoints.MapControllerRoute(
               name: "Unavailable",
               pattern: "/Unavailable",
               defaults: new { controller = "Home", action = "Unavailable" });


                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });
            });


        }
    }
}
