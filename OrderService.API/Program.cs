using OrderService.API.Hubs;
using OrderService.API.Services;
using OrderService.Application.Abstractions;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.Application;
using OrderService.Infraestructure;
using OrderService.Infraestructure.Persistence;
using Serilog;
using Serilog.Enrichers.Span;
using OpenTelemetry.Metrics;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceName = "FastTech.OrdersAPI";

var configuration = builder.Configuration;

var lokiStringConnection = Environment.GetEnvironmentVariable("CONNECTION_LOKI") ??
                "http://localhost:3100";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithSpan()
    .Enrich.WithProperty("Application", serviceName)
    .WriteTo.GrafanaLoki(
        uri: lokiStringConnection,
        labels: new[]
        {
            new LokiLabel { Key = "app", Value = serviceName }
        })
    .CreateLogger();

builder.Host.UseSerilog();

var openTelemetryConnection = Environment.GetEnvironmentVariable("CONNECTION_OPENTELEMETRY") ??
                "http://localhost:4317";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource("MassTransit")
            .AddOtlpExporter(options =>
            {
                // 👇 FORÇANDO A URL E O PROTOCOLO 👇
                options.Endpoint = new Uri(openTelemetryConnection);
                options.Protocol = OtlpExportProtocol.Grpc;
            });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation() // Mede as requisições HTTP
            .AddRuntimeInstrumentation()    // Mede CPU e Memória
            .AddPrometheusExporter();       // Prepara o formato para o Prometheus
    });

builder.Services.AddControllers();
builder.Services
    .AddInfraestructureModule(configuration)
    .AddApplicationModule();

builder.Services.AddSignalR();
builder.Services.AddScoped<IOrderNotificationService, OrderNotificationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var serviceProvider = serviceScope.ServiceProvider;
    DbInitializer.Initialize(serviceProvider);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/orderHub");
app.MapPrometheusScrapingEndpoint();

app.Run();
