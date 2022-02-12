namespace MTBZone.Messaging.Sender
{
    public interface ISender
    {
        void Initialize(string exchange);
        Task Send<T>(T message);
    }
}