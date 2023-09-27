using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Gaming.Quiz.Library.Dependency;
using Microsoft.AspNetCore.HttpOverrides;
using Syncfusion.XlsIO.Implementation;
using Microsoft.Extensions.Hosting;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Feeds;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Gameplay;
using Gaming.Quiz.Interfaces.Leaderboard;
using Gaming.Quiz.Interfaces.Sharing;

namespace Gaming.Quiz.API;

public class Program
{

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://localhost:9877");

        builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        builder.Services.AddMemoryCache();
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddSwagger();
        builder.Services.AddCors();
        builder.Services.AddMvc().AddNewtonsoftJson();
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            // Use the default property (Pascal) casing
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();

        });


        builder.Services.AddScoped<IFeedsBlanket, Gaming.Quiz.Blanket.Feeds.Feeds>();
        builder.Services.AddScoped<ISessionBlanket, Gaming.Quiz.Blanket.Session.Session>();
        builder.Services.AddScoped<IGameplayBlanket, Gaming.Quiz.Blanket.Gameplay.Gameplay>();
        builder.Services.AddScoped<ILeaderboardBlanket, Gaming.Quiz.Blanket.Leaderboard.Leaderboard>();
        builder.Services.AddScoped<IImageGenerationBlanket, Gaming.Quiz.Blanket.Sharing.ImageGeneration>();
        builder.Services.AddScoped<IReCaptchaBlanket, Gaming.Quiz.Blanket.Session.ReCaptcha>();


        var app = builder.Build();
        IRedis redis = app.Services.GetRequiredService<IRedis>();
        IHostApplicationLifetime appLifetime = app.Lifetime;
        appLifetime.RegisterRedis(redis);

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles(new StaticFileOptions() { RequestPath = "/services" });
        //appLifetime.RegisterRedis(redis);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseSwaggerUI(c =>
            {
                //reading swagger json from website's directory location.
                c.SwaggerEndpoint("/services/config/swagger/Gaming.Quiz.API.json", "Gaming.Quiz.API v1");
                c.RoutePrefix = "services/swagger";
            });
        }


        app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();


    }
}
