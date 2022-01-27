namespace CartAPI.Results
{
    public class CartResult
    {
        public long Id { get; set; }
        public List<ItemResult> Items { get; set; }
    }
}
