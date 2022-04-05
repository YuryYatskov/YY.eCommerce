using Catalog.Api.Data;
using Catalog.Api.Repositories;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YY.Catalog.API", Version = "v1" });
    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums();

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    var xmlPathApp = Path.Combine(AppContext.BaseDirectory, xmlFile.Replace("API", "App"));
    c.IncludeXmlComments(xmlPathApp);

});

builder.Services.AddProblemDetails(options => {
    options.IncludeExceptionDetails = (ctx, exception) =>
    {
        var environment = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
        var excludeDetailsFor = new HashSet<Type> {
                    typeof(KeyNotFoundException),
                    typeof(ArgumentException),
                    typeof(ArgumentNullException),
                    typeof(ArgumentOutOfRangeException),
                    typeof(DuplicateWaitObjectException),
                    typeof(HttpRequestException),
                };
        return (environment.IsDevelopment() || environment.IsStaging())
                && !excludeDetailsFor.TryGetValue(exception.GetType(), out Type? valueType);
    };

    options.ShouldLogUnhandledException = (ctx, exception, problemDetails) =>
    {
        if (problemDetails?.Status.HasValue == true && problemDetails.Status.Value < 500)
        {
            // Logging a non-problem server exception as a warning.
            var logger = ctx.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(exception, exception.Message);
            return false;
        }

        // Log the exception as unhandled.
        return true;
    };

    options.ValidationProblemStatusCode = StatusCodes.Status400BadRequest;

    static ProblemDetails Map<TException>(TException exception, HttpStatusCode statusCode) where TException : Exception =>
        new StatusCodeProblemDetails((int)statusCode) { Detail = exception.Message };

    options.Map<KeyNotFoundException>(exception => Map(exception, HttpStatusCode.NotFound));
    options.Map<ArgumentException>(exception => Map(exception, HttpStatusCode.BadRequest));
    options.Map<InvalidOperationException>(exception => Map(exception, HttpStatusCode.BadRequest));
    options.Map<HttpRequestException>(exception => Map(exception, exception.StatusCode ?? 0));
    options.Map<NotImplementedException>(exception => Map(exception, HttpStatusCode.NotImplemented));
    options.Map<Exception>(exception => Map(exception, HttpStatusCode.InternalServerError));
});

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YY.UCProtocol.API v1"));
}
app.UseProblemDetails();

app.UseAuthorization();

app.MapControllers();

app.Run();