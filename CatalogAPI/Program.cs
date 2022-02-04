using CatalogAPI.Data;
using CatalogAPI.EventHandlers.Orders;
using CatalogAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MTBZone.RabbitMQ.Receiver;
using OrdersAPI.Events;
using RabbitMQ.Receiver;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["CatalogAPI:ConnectionString"];

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
builder.Services.AddSingleton<IRabbitMQReceiver, RabbitMQReceiver>();
builder.Services.AddSingleton<IHandler<OrderCreated>, OrderCreatedHandler>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
var orderCreatedHandler = app.Services.GetService<IHandler<OrderCreated>>();
var rabbitMQReceiver = app.Services.GetService<IRabbitMQReceiver>();
rabbitMQReceiver.Receive<OrderCreated, IHandler<OrderCreated>>(orderCreatedHandler, "Order-To-Products", "Orders");

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
