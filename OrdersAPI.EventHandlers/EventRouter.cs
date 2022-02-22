using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CartsAPI.Events;
using MTBZone.Messaging;
using MTBZone.Messaging.Receiver;
using Newtonsoft.Json;
using OrdersAPI.EventHandlers.Carts;

namespace OrdersAPI.EventHandlers
{
    internal class AWSMessage
    {
        public string Message { get; set; }
    }
    public class EventRouter
    {
        private readonly IHandler<CartOrdered> _cartOrderedHandler;

        public EventRouter(IHandler<CartOrdered> cartOrderedHandler)
        {
            _cartOrderedHandler = cartOrderedHandler;
        }
        public async Task Route(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {
                var messageBody = record.Body;
                var messageObject = JsonConvert.DeserializeObject<AWSMessage>(messageBody);
                var messageData = JsonConvert.DeserializeObject<Message>(messageObject.Message);
                Console.WriteLine($"messageData.Type is { messageData.Type}");
                Console.WriteLine($"typeof(CartOrdered).Name is { typeof(CartOrdered).Name}");
                if (messageData.Type == typeof(CartOrdered).Name)
                {
                    var cartOrderedMessage = JsonConvert.DeserializeObject<CartOrdered>(messageObject.Message);
                    await _cartOrderedHandler.Handle(cartOrderedMessage);
                }

            }
        }
    }
}