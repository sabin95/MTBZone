using CartAPI.Data;
using CartAPI.Repository;
using MessagingService.Sender;
using Microsoft.EntityFrameworkCore;
using MTBZone.MessagingService.Sender;

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
    options.UseSqlServer(ConnectionString));
builder.Services.AddScoped<ICartRepository, CartRepository>();
if(environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<ISender, RabbitMQSender>();
}
else
{
    builder.Services.AddSingleton<ISender, SNSSender>();
}
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
var sender = app.Services.GetService<ISender>();
sender.Initialize(cartExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
