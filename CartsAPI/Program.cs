using CartsAPI.Data;
using CartsAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Receiver;
using MTBZone.Messaging.Sender;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var cartsExchange = builder.Configuration["cartsExchange"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CartsContext>(options =>
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Singleton);
builder.Services.AddScoped<ICartsRepository, CartsRepository>();

if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<ISender, RabbitMQSender>();
}
else
{
    builder.Services.AddSingleton<ISender, SNSSender>();
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();
var db = app.Services.GetService<CartsContext>();
db.Database.Migrate();
var sender = app.Services.GetService<ISender>();
sender.Initialize(cartsExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
