using static CartAPI.Utils.Utils;

namespace CartAPI.Data
{
    public class Cart
    {
        public long Id { get; set; }
        public string State { get; set; }
        public List<Item> Items { get; set; }
    }
}
