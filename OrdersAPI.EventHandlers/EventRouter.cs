using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CartsAPI.Events;
using MTBZone.Messaging;
using MTBZone.Messaging.Receiver;
using Newtonsoft.Json;

namespace OrdersAPI.EventHandlers
{
    internal class AWSMessage
    {
        public string Message { get; set; }
    }
    public class EventRouter
    {
        private readonly IHandler<CartOrderedEvent> _cartOrderedHandler;

        public EventRouter(IHandler<CartOrderedEvent> cartOrderedHandler)
        {
            _cartOrderedHandler = cartOrderedHandler;
        }
        public async Task Route(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {
                var messageBody = record.Body;
                var messageObject = JsonConvert.DeserializeObject<AWSMessage>(messageBody);
                var messageData = JsonConvert.DeserializeObject<Event>(messageObject.Message);
                Console.WriteLine($"messageData.Type is { messageData.Type}");
                Console.WriteLine($"typeof(CartOrderedEvent).Name is { typeof(CartOrderedEvent).Name}");
                if (messageData.Type == typeof(CartOrderedEvent).Name)
                {
                    var cartOrderedMessage = JsonConvert.DeserializeObject<CartOrderedEvent>(messageObject.Message);
                    await _cartOrderedHandler.Handle(cartOrderedMessage);
                }

            }
        }
    }
}