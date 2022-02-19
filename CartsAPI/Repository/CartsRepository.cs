using AutoMapper;
using CartsAPI.Commands;
using CartsAPI.Data;
using CartsAPI.Events;
using CartsAPI.Results;
using Microsoft.EntityFrameworkCore;
using MTBZone.Messaging.Sender;

namespace CartsAPI.Repository
{
    public class CartsRepository : ICartsRepository
    {
        private readonly CartsContext _cartsContext;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public CartsRepository(
            CartsContext cartsContext,
            ISender sender,
            IMapper mapper
        )
        {
            _cartsContext = cartsContext;
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<CartResult> CreateCart()
        {
            var cart = new Cart()
            {
                State = Utils.Utils.CartState.Active.ToString()
            };
            _cartsContext.Carts.Add(cart);
            await _cartsContext.SaveChangesAsync();

            var cartResult = _mapper.Map<CartResult>(cart);
            return cartResult;
        }

        public async Task<List<CartResult>> GetAllCartsAsync()
        {
            var carts = await _cartsContext.Carts.Include(o => o.Items).ToListAsync();
            var results = _mapper.Map<List<CartResult>>(carts);
            return results;
        }

        public async Task<CartResult> GetCartById(Guid id)
        {
            var cart = await _cartsContext.Carts.Include(o => o.Items).FirstOrDefaultAsync(c => c.Id == id);
            if (cart == null)
            {
                return null;
            }
            var result = _mapper.Map<CartResult>(cart);

            return result;
        }

        public async Task<ItemResult> AddItemToCart(Guid cartId, AddItemToCartCommand itemCommand)
        {
            if (itemCommand == null)
            {
                throw new ArgumentNullException(nameof(itemCommand), "Cart cannot be null!");
            }
            var cart = await _cartsContext.Carts.Include(o => o.Items).FirstOrDefaultAsync(c => c.Id == cartId);
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
                CartId = cartId,
                ExternalId = itemCommand.ExternalId
            };
            _cartsContext.Items.Add(itemToBeAdded);
            await _cartsContext.SaveChangesAsync();
            var result = _mapper.Map<ItemResult>(itemToBeAdded);
            return result;
        }
        public async Task RemoveItemFromCart(Guid itemId)
        {
            var itemFromCart = await _cartsContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == itemId);
            if (itemFromCart is null)
            {
                throw new ArgumentException(nameof(itemFromCart), "No item found for this id!");
            }
            var item = _mapper.Map<Item>(itemFromCart);
            _cartsContext.Items.Remove(item);
            await _cartsContext.SaveChangesAsync();
        }

        public async Task<CartResult> OrderCart(Guid cartId)
        {
            var cart = await _cartsContext.Carts.Include(o => o.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
            {
                return null;
            }
            cart.State = Utils.Utils.CartState.Ordered.ToString();
            if (cart.Items.Count == 0)
            {
                throw new ArgumentException(nameof(cart.Items.Count), "Cannot make an order if you have no items in the cart!");
            }
            await _cartsContext.SaveChangesAsync();
            var message = new CartOrdered()
            {
                Id = cart.Id,
                Items = cart.Items.Select(x => new CartOrderedItem()
                {
                    Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Title = x.Title,
                    ExternalId = x.ExternalId
                }).ToList()
            };
            await _sender.Send(message);
            var cartResult = _mapper.Map<CartResult>(cart);
            return cartResult;
        }
    }
}
