namespace MTBZone.RabbitMQ.Sender
{
    public interface IRabbitMQSender
    {
        void Initialize(string exchange);
        void Send<T>(T message);
    }
}