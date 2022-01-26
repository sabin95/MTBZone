using CartAPI.Commands;
using CartAPI.Repository;
using CartAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace CartAPI.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            try
            {
                var result = await _cartRepository.CreateCart();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            try
            {
                var results = await _cartRepository.GetAllCartsAsync();
                return Ok(results);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(long id)
        {
            try
            {
                var result = await _cartRepository.GetCartById(id);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartCommand item)
        {
            try
            {
                var result = await _cartRepository.AddItemToCart(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RemoveItemToCart")]
        public async Task<IActionResult> RemoveItemFromCart([FromHeader] long itemId)
        {
            try
            {
                await _cartRepository.RemoveItemFromCart(itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
