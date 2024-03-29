using CartsAPI.Events;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Receiver;
using MTBZone.Messaging.Sender;
using OrdersAPI.Common.Data;
using OrdersAPI.Common.Repository;
using OrdersAPI.EventHandlers.Carts;
using OrdersAPI.Events;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];
var ordersExchange = builder.Configuration["ordersExchange"];
var cartsReceiverQueue = builder.Configuration["cartsReceiverQueue"];
var cartsReceiverExchange = builder.Configuration["cartsReceiverExchange"];
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("OrdersAPI.Common")),
    ServiceLifetime.Singleton
);
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<IReceiver, RabbitMQReceiver>();
    builder.Services.AddSingleton<ISender, RabbitMQSender>();
}
else
{
    builder.Services.AddSingleton<IReceiver, SQSReceiver>();
    builder.Services.AddSingleton<ISender, SNSSender>();
}
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddSingleton<IHandler<CartOrderedEvent>, CartOrderedHandler>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
var db = app.Services.GetService<OrderContext>();
db!.Database.Migrate();
var cartOrderedHandler = app.Services.GetService<IHandler<CartOrderedEvent>>();
if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    var receiver = app.Services.GetService<IReceiver>();
    receiver.Receive<CartOrderedEvent, IHandler<CartOrderedEvent>>(cartOrderedHandler, cartsReceiverQueue, cartsReceiverExchange);
}
var sender = app.Services.GetService<ISender>();
sender!.Initialize(ordersExchange);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
