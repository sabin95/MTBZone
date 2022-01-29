using CartAPI.Events;
using Microsoft.EntityFrameworkCore;
using MTBZone.RabbitMQ.Receiver;
using OrdersAPI.Data;
using OrdersAPI.EventHandlers.Carts;
using OrdersAPI.Repository;
using RabbitMQ.Receiver;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["OrdersAPI:ConnectionString"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderContext>(options => 
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Singleton);
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IRabbitMQReceiver, RabbitMQReceiver>();
builder.Services.AddSingleton<IHandler<CartOrdered>, CartOrderedHandler>();

var app = builder.Build();
var cartOrderedHandler = app.Services.GetService<IHandler<CartOrdered>>();
var rabbitMQReceiver = app.Services.GetService<IRabbitMQReceiver>();
rabbitMQReceiver.Receive<CartOrdered, IHandler<CartOrdered>>(cartOrderedHandler, "Carts-To-Orders", "Carts");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
