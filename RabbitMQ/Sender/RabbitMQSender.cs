using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MTBZone.RabbitMQ.Sender
{
    public class RabbitMQSender : IRabbitMQSender
    {
        private string exchange = "";
        public void Initialize(string exchange)
        {
            this.exchange = exchange;
        }

        public void Send<T>(T message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, "topic");
            channel.BasicPublish(exchange: exchange,
                                          routingKey: typeof(T).Name,
                                          mandatory: true,
                                          basicProperties: null,
                                          body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }
    }
}
