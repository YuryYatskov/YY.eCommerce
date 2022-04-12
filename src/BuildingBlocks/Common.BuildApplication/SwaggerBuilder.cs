using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Common.BuildApplication
{
    public static class SwaggerBuilder
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string titleOpenApi)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title =titleOpenApi, Version = "v1" });
                c.EnableAnnotations();
                c.UseInlineDefinitionsForEnums();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var xmlPathApp = Path.Combine(AppContext.BaseDirectory, xmlFile.Replace("API", "App"));
                c.IncludeXmlComments(xmlPathApp);
            });

            return services;
        }
    }
}
