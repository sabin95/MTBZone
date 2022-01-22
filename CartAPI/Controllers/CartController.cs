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
        public IActionResult CreateCart(CartResult cartResult)
        {
            try
            {
                _cartRepository.CreateCart(cartResult);
                return Ok();
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
