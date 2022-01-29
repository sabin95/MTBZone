namespace RabbitMQ.Receiver
{
    public interface IHandler<T>
    {
        Task Handle (T message);
    }
}
