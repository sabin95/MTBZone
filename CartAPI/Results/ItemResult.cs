namespace CartAPI.Results
{
    public class ItemResult
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public long CartId { get; set; }
        public long ExternalId { get; set; }
    }
}
