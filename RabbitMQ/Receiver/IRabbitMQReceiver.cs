using RabbitMQ.Receiver;

namespace MTBZone.RabbitMQ.Receiver
{
    public interface IRabbitMQReceiver
    {
        void Receive<TMessage, THandler>(THandler handler, string queue, string exchange) where THandler : IHandler<TMessage>;
    }
}