using AspnetRunBasics.Services;
using Common.Logging;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<LoggingDelegatingHandler>();

//Serilog setup
builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy()); 


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//To avoid http rdirection, no warn message in console log
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    // In this case will wait for
    //  2 ^ 1 = 2 seconds then
    //  2 ^ 2 = 4 seconds then
    //  2 ^ 3 = 8 seconds then
    //  2 ^ 4 = 16 seconds then
    //  2 ^ 5 = 32 seconds
    return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, context) =>
                    {
                        Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                    });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );
}