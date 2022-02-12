using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MTBZone.Messaging.Sender
{
    public class RabbitMQSender : ISender
    {
        private string exchange = "";
        public void Initialize(string exchange)
        {
            this.exchange = exchange;
        }

        public Task Send<T>(T message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, "topic",durable:true);
            channel.BasicPublish(exchange: exchange,
                                          routingKey: typeof(T).Name,
                                          mandatory: true,
                                          basicProperties: null,
                                          body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            return Task.CompletedTask;
        }
    }
}
