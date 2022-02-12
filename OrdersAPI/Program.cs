using CartAPI.Events;
using MessagingService.Receiver;
using MessagingService.Sender;
using Microsoft.EntityFrameworkCore;
using MTBZone.MessagingService.Receiver;
using MTBZone.MessagingService.Sender;
using OrdersAPI.Data;
using OrdersAPI.EventHandlers.Carts;
using OrdersAPI.Repository;

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
    ServiceLifetime.Singleton);
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
if(environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<IReceiver, RabbitMQReceiver>();
    builder.Services.AddSingleton<ISender, RabbitMQSender>();
}
else
{
    builder.Services.AddSingleton<IReceiver, SQSReceiver>();
    builder.Services.AddSingleton<ISender, SNSSender>();
}
builder.Services.AddSingleton<IHandler<CartOrdered>, CartOrderedHandler>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
var cartOrderedHandler = app.Services.GetService<IHandler<CartOrdered>>();
var receiver = app.Services.GetService<IReceiver>();
receiver.Receive<CartOrdered, IHandler<CartOrdered>>(cartOrderedHandler, cartsReceiverQueue, cartsReceiverExchange);
var sender = app.Services.GetService<ISender>();
sender.Initialize(odersExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
