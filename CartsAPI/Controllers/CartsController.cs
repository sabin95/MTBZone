using CartsAPI.Commands;
using CartsAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CartsAPI.Controllers
{
    [Route("api/[controller]")]
    public class CartsController : Controller
    {
        private readonly ICartsRepository _cartsRepository;

        public CartsController(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            try
            {
                var result = await _cartsRepository.CreateCart();
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
                var results = await _cartsRepository.GetAllCartsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartById(Guid cartId)
        {
            try
            {
                var result = await _cartsRepository.GetCartById(cartId);
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

        [HttpPut("{cartId}/Order")]
        public async Task<ActionResult> OrderCart([FromRoute] Guid cartId)
        {
            try
            {
                var result = await _cartsRepository.OrderCart(cartId);
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

        [HttpPut("{cartId}/Items/Add")]
        public async Task<IActionResult> AddItemToCart(Guid cartId, [FromBody] AddItemToCartCommand item)
        {
            try
            {
                var result = await _cartsRepository.AddItemToCart(cartId, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Items/{itemId}/Remove")]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid itemId)
        {
            try
            {
                await _cartsRepository.RemoveItemFromCart(itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
