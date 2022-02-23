using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using MTBZone.Messaging;
using MTBZone.Messaging.Receiver;
using Newtonsoft.Json;
using OrdersAPI.Events;

namespace CatalogAPI.EventHandlers
{
    internal class AWSMessage
    {
        public string Message { get; set; }
    }
    public class EventRouter
    {
        private readonly IHandler<OrderCreatedEvent> _orderCartHandler;

        public EventRouter(IHandler<OrderCreatedEvent> orderCartHandler)
        {
            _orderCartHandler = orderCartHandler;
        }
        public async Task Route(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {
                var messageBody = record.Body;
                var messageObject = JsonConvert.DeserializeObject<AWSMessage>(messageBody);
                var messageData = JsonConvert.DeserializeObject<Event>(messageObject.Message);
                Console.WriteLine($"messageData.Type is { messageData.Type}");
                Console.WriteLine($"typeof(OrderCreatedEvent).Name is { typeof(OrderCreatedEvent).Name}");
                if (messageData.Type == typeof(OrderCreatedEvent).Name)
                {
                    var orderCartMessage = JsonConvert.DeserializeObject<OrderCreatedEvent>(messageObject.Message);
                    await _orderCartHandler.Handle(orderCartMessage);
                }

            }
        }
    }
}
