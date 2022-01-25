using CartAPI.Commands;
using CartAPI.Results;

namespace CartAPI.Repository
{
    public interface ICartRepository
    {
        public void CreateCart(CartResult cartResult);
        public Task<List<CartResult>> GetAllCartsAsync();
        public Task<CartResult> GetCartById(long id);
        public Task AddItemToCart(ItemCommand itemCommand, long cartId);
        public void RemoveItemFromCart(long itemId, long cartId);
    }
}
