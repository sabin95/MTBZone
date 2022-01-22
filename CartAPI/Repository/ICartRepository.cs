using CartAPI.Results;

namespace CartAPI.Repository
{
    public interface ICartRepository
    {
        public void CreateCart(CartResult cartResult);
        public Task<List<CartResult>> GetAllCartsAsync();
        public Task<CartResult> GetCartById(long id);
    }
}
