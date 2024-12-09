using App.API.Interfaces;
using App.API.Repositories;
using App.API.Services;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using App.API.Middlewares;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using AutoMapper;
using App.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("DB"));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>(); 
builder.Services.AddScoped<CustomerService>();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});
var mapperConfig = new MapperConfiguration(map =>
{
    map.AddProfile<CustomerProfile>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();//AddPrometheusExporter
    })
    .WithTracing(tracing =>
    {
        if (builder.Environment.IsDevelopment()) tracing.SetSampler(new AlwaysOnSampler());// We want to view all traces in development

        tracing.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();//AddJaegerExporter
    });
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() 
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseExceptionHandling();
app.UseCors(a => a.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();

app.Run();
public partial class Program { }






