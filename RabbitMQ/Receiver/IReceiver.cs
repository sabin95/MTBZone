using MessagingService.Receiver;

namespace MTBZone.MessagingService.Receiver
{
    public interface IReceiver
    {
        void Receive<TMessage, THandler>(THandler handler, string queue, string exchange) where THandler : IHandler<TMessage>;
    }
}