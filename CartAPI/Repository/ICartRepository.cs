using CartAPI.Commands;
using CartAPI.Results;

namespace CartAPI.Repository
{
    public interface ICartRepository
    {
        public Task CreateCart(CartResult cartResult);
        public Task<List<CartResult>> GetAllCartsAsync();
        public Task<CartResult> GetCartById(long id);
        public Task AddItemToCart(ItemCommand itemCommand, long cartId);
        public Task RemoveItemFromCart(long itemId, long cartId);
    }
}
