using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;

namespace MTBZone.Messaging.Sender
{
    public class SNSSender : ISender
    {
        private string _exchange = "";
        public void Initialize(string exchange)
        {
            _exchange = exchange;
        }

        public async Task Send<T>(T message)
        {
            var snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.EUCentral1);
            var request = new PublishRequest
            {
                TopicArn = _exchange,
                Message = JsonConvert.SerializeObject(message)
            };

            await snsClient.PublishAsync(request);
        }
    }
}
