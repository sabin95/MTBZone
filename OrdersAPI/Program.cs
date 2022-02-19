using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Sender;
using OrdersAPI.Common.Data;
using OrdersAPI.Common.Repository;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];
var odersExchange = builder.Configuration["odersExchange"];
var cartsReceiverQueue = builder.Configuration["cartsReceiverQueue"];
var cartsReceiverExchange = builder.Configuration["cartsReceiverExchange"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Singleton
);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ISender, SNSSender>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();
var sender = app.Services.GetService<ISender>();
sender!.Initialize(odersExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();