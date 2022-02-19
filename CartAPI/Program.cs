using CartAPI.Data;
using CartAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Sender;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var cartExchange = builder.Configuration["cartExchange"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CartContext>(options =>
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Scoped
);
builder.Services.AddScoped<ICartRepository, CartRepository>();
// builder.Services.AddScoped<ISender, SNSSender>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();
// var sender = app.Services.GetService<ISender>();
// sender.Initialize(cartExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
