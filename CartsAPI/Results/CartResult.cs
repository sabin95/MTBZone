using static CartsAPI.Utils.Utils;

namespace CartsAPI.Results
{
    public class CartResult
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<ItemResult> Items { get; set; }
    }
}
