using Basket.Api.Repositories;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "YY.Basket.API", Version = "v1" });
    options.EnableAnnotations();
    options.UseInlineDefinitionsForEnums();

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    var xmlPathApp = Path.Combine(AppContext.BaseDirectory, xmlFile.Replace("API", "App"));
    options.IncludeXmlComments(xmlPathApp);
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

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetSection(nameof(RedisCacheOptions))
    .Get<RedisCacheOptions>().Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

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