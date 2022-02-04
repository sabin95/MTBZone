namespace OrdersAPI.Results
{
    public class OrderResult
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<ItemResult> Items { get; set; }
    }
}
