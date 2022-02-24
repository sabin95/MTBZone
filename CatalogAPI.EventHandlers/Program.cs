using CatalogAPI.Common.Data;
using CatalogAPI.EventHandlers.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTBZone.Messaging.Receiver;
using OrdersAPI.Events;
using Microsoft.EntityFrameworkCore;
using CatalogAPI.Common.Repository;
using MTBZone.Messaging.Sender;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.RuntimeSupport;

namespace CatalogAPI.EventHandlers
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();
            var ConnectionString = config["ConnectionString"];
            var ordersExchange = config["ordersExchange"];
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IHandler<OrderCreatedEvent>, OrderCreatedHandler>();
            services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(ConnectionString),
                ServiceLifetime.Singleton);
            services.AddSingleton<EventRouter>();
            services.AddSingleton<ICategoriesRepository, CategoriesRepository>();
            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddAutoMapper(typeof(Program));

            var provider = services.BuildServiceProvider();
            var handler = provider.GetService<EventRouter>().Route;


            await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
                .Build()
                .RunAsync();
        }
    }
}
