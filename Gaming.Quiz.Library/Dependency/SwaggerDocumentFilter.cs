using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using  Gaming.Quiz.Contracts.Configuration;
using Microsoft.OpenApi.Models;

namespace  Gaming.Quiz.Library.Dependency
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        protected readonly String _BasePath;

        public SwaggerDocumentFilter(IOptions<Application> appSettings, IHostingEnvironment env)
        {
            //for local machine comment this out
            //_BasePath = appSettings.Value.CustomSwaggerConfig.BasePath;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext documentFilterContext)
        {
            //for local machine comment this out
            //swaggerDoc.BasePath = _BasePath;
            swaggerDoc.Servers.Add(new OpenApiServer() { Url = _BasePath });
            //swaggerDoc.Host1 = "some-url-that-is-hosted-on-azure.azurewebsites.net";
            //swaggerDoc.Schemes = new List<string> { "https" };
        }

       
    }
}
