using CartAPI.Commands;
using CartAPI.Results;

namespace CartAPI.Repository
{
    public interface ICartRepository
    {
        public Task<CartResult> CreateCart();
        public Task<List<CartResult>> GetAllCartsAsync();
        public Task<CartResult> GetCartById(Guid id);
        public Task<ItemResult> AddItemToCart(AddItemToCartCommand itemCommand);
        public Task RemoveItemFromCart(Guid itemId);
        public Task<CartResult> OrderCart(Guid cartId);
    }
}
