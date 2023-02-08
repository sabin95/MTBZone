using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MTBZone.Messaging.Receiver
{
    public class RabbitMQReceiver : IReceiver
    {

        public void Receive<TMessage, THandler>(THandler handler, string queue, string exchange) where THandler : IHandler<TMessage>
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange, "topic", durable: true);

            channel.QueueDeclare(queue: queue,
                               durable: true,
                               exclusive: false,
                               autoDelete: true,
                               arguments: null);
            channel.QueueBind(queue: queue,
                            exchange: exchange,
                            routingKey: typeof(TMessage).Name,
                            arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageObject = JsonConvert.DeserializeObject<TMessage>(message);
                await handler.Handle(messageObject);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: queue,
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}
