namespace MTBZone.Messaging.Receiver
{
    public interface IHandler<T>
    {
        Task Handle (T message);
    }
}
