using CartAPI.Data;
using CartAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MTBZone.RabbitMQ.Sender;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["CartAPI:ConnectionString"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CartContext>(options =>
    options.UseSqlServer(ConnectionString));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IRabbitMQSender, RabbitMQSender>();

var app = builder.Build();
var rabbitMQ = app.Services.GetService<IRabbitMQSender>();
rabbitMQ.Initialize("Carts");

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
