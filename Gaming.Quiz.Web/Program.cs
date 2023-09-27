using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Gaming.Quiz.Library.Dependency;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Gaming.Quiz.Interfaces.Admin;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Contracts.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:9876");


builder.Services.AddRazorPages();
builder.Services.AddServices(builder.Configuration);

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
});
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();

//User-defined extension method
var app = builder.Build();
IHostApplicationLifetime appLifetime = app.Lifetime;
IRedis redis = app.Services.GetRequiredService<IRedis>();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
appLifetime.RegisterRedis(redis);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
                    name: "Default",
                    pattern: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
               name: "terms",
               pattern: "/terms",
               defaults: new { controller = "Home", action = "Terms" });

app.MapRazorPages();

app.Run();