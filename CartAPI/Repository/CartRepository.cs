using CartAPI.Commands;
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

        public async Task<CartResult> CreateCart()
        {
            var cart = new Cart();
            _cartContext.Carts.Add(cart);
            await _cartContext.SaveChangesAsync();

            var cartResult = new CartResult()
            {
                Id = cart.Id
            };
            return cartResult;
        }

        public async Task<List<CartResult>> GetAllCartsAsync()
        {
            var results = await _cartContext.Carts.Select(c => new CartResult()
            {
                Id = c.Id
            }).ToListAsync();
            foreach(var result in results)
            {
                result.Items = await GetItemsByCartId(result.Id);
            }
            return results;
        }

        public async Task<CartResult> GetCartById(long id)
        {
            var result = await _cartContext.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (result == null)
            {
                return null;
            }
            var itemsForCart = await GetItemsByCartId(id);
            var cart = new CartResult()
            {
                Id = result.Id,
                Items = itemsForCart.ToList()
            };
            return cart;
        }

        public async Task<ItemResult> AddItemToCart(AddItemToCartCommand itemCommand)
        {
            if (itemCommand == null)
            {
                throw new ArgumentNullException(nameof(itemCommand), "Cart cannot be null!");
            }
            var cart = await GetCartById(itemCommand.CartId);
            if (cart is null)
            {
                throw new ArgumentException(nameof(cart), "No cart exists for this id!");
            }
            var itemToBeAdded = new Item()
            {
                Title = itemCommand.Title,
                Price = itemCommand.Price,
                Quantity = itemCommand.Quantity,
                CartId = itemCommand.CartId
            };
            _cartContext.Items.Add(itemToBeAdded);
            await _cartContext.SaveChangesAsync();
            var result = new ItemResult()
            {
                Id=itemToBeAdded.Id,
                Title=itemToBeAdded.Title,
                Price=itemToBeAdded.Price,
                Quantity=itemToBeAdded.Quantity,
                CartId=itemToBeAdded.CartId
            };
            return result;
        }
        public async Task RemoveItemFromCart(long itemId)
        {
            if (itemId<0)
            {
                throw new ArgumentException(nameof(itemId), "Cart id must be greater than 0!");
            }
            var itemFromCart = await _cartContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == itemId);
            if (itemFromCart is null)
            {
                throw new ArgumentException(nameof(itemFromCart), "No item found for this id!");
            }
            var item = new Item()
            {
                Id = itemFromCart.Id,
                Title = itemFromCart.Title,
                Price = itemFromCart.Price,
                Quantity = itemFromCart.Quantity,
                CartId = itemFromCart.CartId
            };
            _cartContext.Items.Remove(item);
            await _cartContext.SaveChangesAsync();
        }

        private async Task<List<ItemResult>> GetItemsByCartId(long cartId)
        {
            if (cartId < 0)
            {
                throw new ArgumentException(nameof(cartId), "Cart id must be greater than 0!");
            }
            var cartResult = await _cartContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
            if (cartResult is null)
            {
                throw new ArgumentException(nameof(cartResult), "No cart found for this id!");
            }
            var items = await _cartContext.Items.Where(x=>x.CartId == cartId).ToListAsync();
            var itemsResult = new List<ItemResult>();
            foreach(var item in items)
            {
                itemsResult.Add(new ItemResult()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    CartId = item.CartId
                });
            }
            return itemsResult;
        }
    }
}
