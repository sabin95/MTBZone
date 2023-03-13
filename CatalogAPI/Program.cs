using CatalogAPI.Common.Data;
using CatalogAPI.Common.Repository;
using CatalogAPI.EventHandlers.Orders;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Receiver;
using MTBZone.Messaging.Sender;
using OrdersAPI.Events;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration["ConnectionString"];
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];
var ordersReceiverQueue = builder.Configuration["ordersReceiverQueue"];
var ordersReceiverExchange = builder.Configuration["ordersReceiverExchange"];
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("CatalogAPI.Common")),
    ServiceLifetime.Singleton
);
builder.Services.AddTransient<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddTransient<IProductsRepository, ProductsRepository>();

if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    builder.Services.AddSingleton<IReceiver, RabbitMQReceiver>();
}
else
{
    builder.Services.AddSingleton<IReceiver, SQSReceiver>();
}

builder.Services.AddSingleton<IHandler<OrderCreatedEvent>, OrderCreatedHandler>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);
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
var orderCreatedHandler = app.Services.GetService<IHandler<OrderCreatedEvent>>();
if (environment.ToUpper().Equals("DEVELOPMENT"))
{
    var receiver = app.Services.GetService<IReceiver>();
    receiver.Receive<OrderCreatedEvent, IHandler<OrderCreatedEvent>>(orderCreatedHandler, ordersReceiverQueue, ordersReceiverExchange);
}
var db = app.Services.GetService<CatalogContext>();
db.Database.Migrate();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
