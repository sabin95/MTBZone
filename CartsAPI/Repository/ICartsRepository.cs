using CartsAPI.Commands;
using CartsAPI.Results;

namespace CartsAPI.Repository
{
    public interface ICartsRepository
    {
        public Task<CartResult> CreateCart();
        public Task<List<CartResult>> GetAllCartsAsync();
        public Task<CartResult> GetCartById(Guid id);
        public Task<ItemResult> AddItemToCart(Guid cartId, AddItemToCartCommand itemCommand);
        public Task RemoveItemFromCart(Guid itemId);
        public Task<CartResult> OrderCart(Guid cartId);
    }
}
