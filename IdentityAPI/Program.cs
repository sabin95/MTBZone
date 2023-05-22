using IdentityAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration["ConnectionString"];

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(ConnectionString),
    ServiceLifetime.Singleton);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
