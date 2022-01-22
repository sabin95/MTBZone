using CartAPI.Data;
using CartAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly CartContext _cartContext;

        public CartRepository(CartContext cartContext)
        {
            _cartContext = cartContext;
        }
        public void CreateCart(CartResult cartResult)
        {
            if (cartResult == null)
            {
                throw new ArgumentNullException(nameof(cartResult),"Cart cannot be null!");
            }
            var cart = new Cart()
            {
                Id = cartResult.Id
            };
            _cartContext.Carts.Add(cart);
            _cartContext.SaveChangesAsync();
        }

        public async Task<List<CartResult>> GetAllCartsAsync()
        {
            var results = await _cartContext.Carts.Select(c => new CartResult()
            {
                Id = c.Id
            }).ToListAsync();
            return results;
        }

        public async Task<CartResult> GetCartById(long id)
        {
            var result = await _cartContext.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (result == null)
            {
                return null;
            }
            var cart = new CartResult()
            {
                Id = result.Id
            };
            return cart;
        }
    }
}
