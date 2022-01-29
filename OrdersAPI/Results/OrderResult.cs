namespace OrdersAPI.Results
{
    public class OrderResult
    {
        public long Id { get; set; }
        public string State { get; set; }
        public List<ItemResult> Items { get; set; }
    }
}
