using CartAPI.Commands;
using CartAPI.Data;
using CartAPI.Events;
using CartAPI.Results;
using Microsoft.EntityFrameworkCore;
using MTBZone.RabbitMQ.Sender;

namespace CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly CartContext _cartContext;
        private readonly IRabbitMQSender _rabbitMQSender;

        public CartRepository(CartContext cartContext,IRabbitMQSender rabbitMQSender)
        {
            _cartContext = cartContext;
            _rabbitMQSender = rabbitMQSender;
        }       

        public async Task<CartResult> CreateCart()
        {
            var cart = new Cart()
            {
                State= Utils.Utils.CartState.Active.ToString()
            };
            _cartContext.Carts.Add(cart);
            await _cartContext.SaveChangesAsync();

            var cartResult = new CartResult()
            {
                Id = cart.Id,
                State= cart.State.ToString()
            };
            return cartResult;
        }

        public async Task<List<CartResult>> GetAllCartsAsync()
        {
            var results = await _cartContext.Carts.Include(o=>o.Items).Select(c => new CartResult()
            {
                Id = c.Id,
                State = c.State.ToString(),
                Items = c.Items.Select(ci=>new ItemResult()
                {
                    Id = ci.Id,
                    Title = ci.Title,
                    Price = ci.Price,
                    Quantity = ci.Quantity,
                    CartId = ci.CartId,
                    ExternalId = ci.ExternalId
                }).ToList()
            }).ToListAsync();
            return results;
        }

        public async Task<CartResult> GetCartById(Guid id)
        {
            var result = await _cartContext.Carts.Include(o=>o.Items).FirstOrDefaultAsync(c => c.Id == id);
            if (result == null)
            {
                return null;
            }
            var cart = new CartResult()
            {
                Id = result.Id,
                State = result.State.ToString(),
                Items = result.Items.Select(ci => new ItemResult()
                {
                    Id = ci.Id,
                    Title = ci.Title,
                    Price = ci.Price,
                    Quantity = ci.Quantity,
                    CartId = ci.CartId,
                    ExternalId = ci.ExternalId
                }).ToList()
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
            //TO DO : verify if product exists for externalId
            var itemToBeAdded = new Item()
            {
                Title = itemCommand.Title,
                Price = itemCommand.Price,
                Quantity = itemCommand.Quantity,
                CartId = itemCommand.CartId,
                ExternalId = itemCommand.ExternalId
            };
            _cartContext.Items.Add(itemToBeAdded);
            await _cartContext.SaveChangesAsync();
            var result = new ItemResult()
            {
                Id=itemToBeAdded.Id,
                Title=itemToBeAdded.Title,
                Price=itemToBeAdded.Price,
                Quantity=itemToBeAdded.Quantity,
                CartId=itemToBeAdded.CartId,
                ExternalId=itemToBeAdded.ExternalId
            };
            return result;
        }
        public async Task RemoveItemFromCart(Guid itemId)
        {
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
                CartId = itemFromCart.CartId,
                ExternalId = itemFromCart.ExternalId
            };
            _cartContext.Items.Remove(item);
            await _cartContext.SaveChangesAsync();
        }

        public async Task<CartResult> OrderCart(Guid cartId)
        {
            var cart = await GetCartById(cartId);
            if (cart == null)
            {
                return null;
            }
            cart.State = Utils.Utils.CartState.Ordered.ToString();
            if(cart.Items.Count==0)
            {
                throw new ArgumentException(nameof(cart.Items.Count), "Cannot make an order if you have no items in the cart!");
            }
            await _cartContext.SaveChangesAsync();
            var message = new CartOrdered()
            {
                Id=cart.Id,
                Items = cart.Items.Select(x=>new CartOrderedItem()
                    {
                    Id = x.Id,
                    Price = x.Price,
                    Quantity=x.Quantity,
                    Title=x.Title,
                    ExternalId=x.ExternalId
                }).ToList()
            };
            _rabbitMQSender.Send(message);
            var cartResult = new CartResult()
            {
                Id = cart.Id,
                State = cart.State,
                Items = cart.Items.ToList()
            };
            return cartResult;
        }
    }
}
