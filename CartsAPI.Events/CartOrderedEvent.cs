
using MTBZone.Messaging;

namespace CartsAPI.Events
{
    public class CartOrderedEvent : Event
    {
        public Guid Id { get; set; }
        public List<CartOrderedItem> Items { get; set; }
        public override string Type => typeof(CartOrderedEvent).Name;
    }
    public class CartOrderedItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public Guid ExternalId { get; set; }
    }
}
