using Common.BuildApplication;
using EventBus.Messages.Common;
using Hellang.Middleware.ProblemDetails;
using MassTransit;
using Microsoft.Toolkit.Diagnostics;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

const string _nameService = "YY.Ordering.API";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger(_nameService);

builder.Services.AddExceptionHandlerProblemDetail();

builder.Services.AddMassTransit(config => {
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQuene, c => c.ConfigureConsumer<BasketCheckoutConsumer>(ctx));
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<BasketCheckoutConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<OrderContext>((context, service) => {
    var logger = service.GetService<ILogger<OrderContextSeed>>();
    Guard.IsNotNull(logger, nameof(logger));
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

app.Run();