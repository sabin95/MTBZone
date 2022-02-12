namespace MessagingService.Receiver
{
    public interface IHandler<T>
    {
        Task Handle (T message);
    }
}
