using AutoMapper;
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
        private readonly IMapper _mapper;

        public CartRepository(CartContext cartContext,IRabbitMQSender rabbitMQSender, IMapper mapper)
        {
            _cartContext = cartContext;
            _rabbitMQSender = rabbitMQSender;
            _mapper = mapper;
        }       

        public async Task<CartResult> CreateCart()
        {
            var cart = new Cart()
            {
                State= Utils.Utils.CartState.Active.ToString()
            };
            _cartContext.Carts.Add(cart);
            await _cartContext.SaveChangesAsync();

            var cartResult = _mapper.Map<CartResult>(cart);
            return cartResult;
        }

        public async Task<List<CartResult>> GetAllCartsAsync()
        {
            var results = _mapper.Map<List<CartResult>>(await _cartContext.Carts.Include(o => o.Items).ToListAsync());
            return results;
        }

        public async Task<CartResult> GetCartById(Guid id)
        {
            var result = _mapper.Map<CartResult>(await _cartContext.Carts.Include(o => o.Items).FirstOrDefaultAsync(c => c.Id == id));
            if (result == null)
            {
                return null;
            }
            return result;
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
            var result = _mapper.Map<ItemResult>(itemToBeAdded);
            return result;
        }
        public async Task RemoveItemFromCart(Guid itemId)
        {
            var itemFromCart = await _cartContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == itemId);
            if (itemFromCart is null)
            {
                throw new ArgumentException(nameof(itemFromCart), "No item found for this id!");
            }
            var item = _mapper.Map<Item>(itemFromCart);
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
            var cartResult = _mapper.Map<CartResult>(cart);
            return cartResult;
        }
    }
}
