using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gaming.Quiz.Contracts.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Gaming.Quiz.Library.Dependency;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Gaming.Quiz.Interfaces.Admin;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Interfaces.Feeds;
using Gaming.Quiz.Interfaces.DataPopulation;
using Gaming.Quiz.Interfaces.Leaderboard;
using Gaming.Quiz.Interfaces.PointCalculation;
using Gaming.Quiz.Interfaces.GamedayMapping;
using Gaming.Quiz.Interfaces.Template;
using Gaming.Quiz.Interfaces.Analytics;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:9882");


builder.Services.AddRazorPages();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddSingleton<ISession, Gaming.Quiz.Admin.App_Code.Authorization>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
});
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IDataPopulationBlanket, Gaming.Quiz.Blanket.DataPopluation.DataPopulation>();
builder.Services.AddScoped<IAdminServicesBlanket, Gaming.Quiz.Blanket.Admin.AdminServices>();
builder.Services.AddScoped<IIngestionBlanket, Gaming.Quiz.Blanket.Feeds.Ingestion>();
builder.Services.AddScoped<IFeedsBlanket, Gaming.Quiz.Blanket.Feeds.Feeds>();
builder.Services.AddScoped<ILeaderboardBlanket, Gaming.Quiz.Blanket.Leaderboard.Leaderboard>();
builder.Services.AddScoped<IPointCalculationBlanket, Gaming.Quiz.Blanket.PointCalculation.PointCalculation>();
builder.Services.AddScoped<IGamedayMappingBlanket, Gaming.Quiz.Blanket.GamedayMapping.GamedayMapping>();
builder.Services.AddScoped<ITemplateBlanket, Gaming.Quiz.Blanket.Template.Template>();
builder.Services.AddScoped<IAnalyticsBlanket, Gaming.Quiz.Blanket.Analytics.Analytics>();


//User-defined extension method
var app = builder.Build();
IHostApplicationLifetime appLifetime = app.Lifetime;
IRedis redis = app.Services.GetRequiredService<IRedis>();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions() { RequestPath = "/games/quiz/admin" });
appLifetime.RegisterRedis(redis);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "games/quiz/admin",
    pattern: "games/quiz/admin/{action}/{id?}",
    defaults: new { Controller = "Home" });

app.MapRazorPages();

app.Run();