using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Using the GetValue<type>(string key) method
var redisServer = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
// or using the index property (which always returns a string)
//var configValue = builder.Configuration["Authentication:CookieAuthentication:LoginPath"];

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisServer;
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
