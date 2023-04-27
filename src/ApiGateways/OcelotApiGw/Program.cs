
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//Serilog setup
builder.Host.UseSerilog(SeriLogger.Configure);

//IConfiguration configuration = builder.Configuration;
//builder.Configuration.GetValue<string>("EventBusSettings:HostAddress")
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
});

//var configuration = builder.Configuration.GetSection("Logging");
//builder.Logging.AddConfiguration(configuration);
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());
   
var app = builder.Build();
//AddConfiguration(this ILoggingBuilder builder, IConfiguration configuration)
app.MapGet("/", () => "Hello World!");
await app.UseOcelot();
app.Run();

