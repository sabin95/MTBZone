using CatalogAPI.Data;
using CatalogAPI.Repository;
using Microsoft.EntityFrameworkCore;

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
    ServiceLifetime.Singleton
);
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services
  .AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();
var db = app.Services.GetService<CatalogContext>();
db.Database.Migrate();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
