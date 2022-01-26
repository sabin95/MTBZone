﻿using CartAPI.Commands;
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

        public async Task CreateCart(CartResult cartResult)
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
            await _cartContext.SaveChangesAsync();
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

        public async Task AddItemToCart(ItemCommand itemCommand, long cartId)
        {
            if (itemCommand == null)
            {
                throw new ArgumentNullException(nameof(itemCommand), "Cart cannot be null!");
            }
            if (cartId<0)
            {
                throw new ArgumentException(nameof(cartId), "Cart id must be greater than 0!");
            }
            var cart = await GetCartById(cartId);
            if (cart is null)
            {
                throw new ArgumentException(nameof(cart), "No cart exists for this id!");
            }
            var itemToBeAdded = new Item()
            {
                Id = itemCommand.Id,
                Title = itemCommand.Title,
                Price = itemCommand.Price,
                Quantity = itemCommand.Quantity,
                CartId = cartId
            };
            _cartContext.Items.Add(itemToBeAdded);
            bool hasChanges = _cartContext.ChangeTracker.HasChanges();
            await _cartContext.SaveChangesAsync();
        }
        public async Task RemoveItemFromCart(long itemId, long cartId)
        {
            if (cartId < 0)
            {
                throw new ArgumentException(nameof(cartId), "Cart id must be greater than 0!");
            }
            if (itemId<0)
            {
                throw new ArgumentException(nameof(itemId), "Cart id must be greater than 0!");
            }
            var itemFromCart = await GetItemById(itemId);
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

        private async Task<ItemResult> GetItemById(long id)
        {
            if (id < 0)
            {
                throw new ArgumentException(nameof(id), "Cart id must be greater than 0!");
            }
            var result = await _cartContext.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                throw new ArgumentException(nameof(result), "No item found for this id!");
            }
            var item = new ItemResult()
            {
                Id = result.Id,
                Title = result.Title,
                Price=result.Price,
                Quantity=result.Quantity,
                CartId=result.CartId
            };
            return item;
        }
    }
}