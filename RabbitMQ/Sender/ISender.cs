namespace MTBZone.MessagingService.Sender
{
    public interface ISender
    {
        void Initialize(string exchange);
        Task Send<T>(T message);
    }
}