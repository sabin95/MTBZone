using CatalogAPI.Data;
using CatalogAPI.EventHandlers.Orders;
using CatalogAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Receiver;
using OrdersAPI.Events;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];
var ordersReceiverQueue = builder.Configuration["ordersReceiverQueue"];
var ordersReceiverExchange = builder.Configuration["ordersReceiverExchange"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Singleton);
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<IReceiver, RabbitMQReceiver>();
}
else
{
    builder.Services.AddSingleton<IReceiver, SQSReceiver>();
}
builder.Services.AddSingleton<IHandler<OrderCreated>, OrderCreatedHandler>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);


var app = builder.Build();
var orderCreatedHandler = app.Services.GetService<IHandler<OrderCreated>>();
var receiver = app.Services.GetService<IReceiver>();
receiver.Receive<OrderCreated, IHandler<OrderCreated>>(orderCreatedHandler, ordersReceiverQueue, ordersReceiverExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
