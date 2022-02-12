using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using MTBZone.MessagingService.Sender;
using Newtonsoft.Json;

namespace MessagingService.Sender
{
    public class SNSSender : ISender
    {
        private AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.EUCentral1);
        private string _exchange = "";
        public void Initialize(string exchange)
        {
            _exchange = exchange;
        }

        public async Task Send<T>(T message)
        {
            var request = new PublishRequest
            {
                TopicArn = _exchange,
                Message = JsonConvert.SerializeObject(message)
            };

            await snsClient.PublishAsync(request);
        }
    }
}
