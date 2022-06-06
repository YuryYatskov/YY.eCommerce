using Basket.Api.Repositories;
using Basket.Api.Services;
using Common.BuildApplication;
using Discount.Grpc.Protos;
using Hellang.Middleware.ProblemDetails;
using MassTransit;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Serilog;

const string _nameService = "YY.Basket.API";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger(_nameService);

builder.Services.AddExceptionHandlerProblemDetail();

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetSection(nameof(RedisCacheOptions))
    .Get<RedisCacheOptions>().Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddMassTransit(config => config.UsingRabbitMq((_, cfg) =>
    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"])));
builder.Services.AddMassTransitHostedService();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_nameService} v1"));
}
app.UseProblemDetails();

app.UseAuthorization();

app.MapControllers();

app.Run();