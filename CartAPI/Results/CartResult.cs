using static CartAPI.Utils.Utils;

namespace CartAPI.Results
{
    public class CartResult
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public List<ItemResult> Items { get; set; }
    }
}
