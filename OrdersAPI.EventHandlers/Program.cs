using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace OrdersAPI.EventHandlers
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var handler = new Handler().Handle;
            await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
                .Build()
                .RunAsync();
        }
    }
}
