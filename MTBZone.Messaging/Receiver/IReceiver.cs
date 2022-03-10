namespace MTBZone.Messaging.Receiver
{
    public interface IReceiver
    {
        void Receive<TMessage, THandler>(THandler handler, string queue, string exchange) where THandler : IHandler<TMessage>;
    }
}