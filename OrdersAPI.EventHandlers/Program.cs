using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using CartsAPI.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTBZone.Messaging.Receiver;
using MTBZone.Messaging.Sender;
using OrdersAPI.Common.Data;
using OrdersAPI.Common.Repository;
using OrdersAPI.EventHandlers.Carts;

namespace OrdersAPI.EventHandlers
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json",true)
                .Build();
            var ConnectionString = config["ConnectionString"];
            var ordersExchange = config["ordersExchange"];
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IHandler<CartOrderedEvent>, CartOrderedHandler>();
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(ConnectionString),
                ServiceLifetime.Singleton);
            services.AddSingleton<EventRouter>();
            services.AddSingleton<IOrderRepository, OrderRepository>();services.AddSingleton<ISender, RabbitMQSender>();
            services.AddAutoMapper(typeof(Program));

            var provider = services.BuildServiceProvider();   
            var handler = provider.GetService<EventRouter>().Route;
            var sender = provider.GetService<ISender>();
            sender!.Initialize(ordersExchange);


            await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
                .Build()
                .RunAsync();
        }
    }
}
