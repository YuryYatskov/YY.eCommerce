using Common.BuildApplication;
using Discount.Api.Repositories;
using Hellang.Middleware.ProblemDetails;
using Serilog;

const string _nameService = "YY.Discount.API";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger(_nameService);
builder.Services.AddExceptionHandlerProblemDetail();

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_nameService } v1"));
}
app.UseProblemDetails();

app.UseAuthorization();

app.MapControllers();

app.Run();
