using CatalogAPI.Commands;
using CatalogAPI.Repository;
using CatalogAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(ProductCommand productCommand)
        {
            try
            {
                var result = await _productsRepository.AddProduct(productCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productsRepository.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await _productsRepository.GetProductById(productId);
                if (product is null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProductById(Guid productId, ProductCommand productCommand)
        {
            try
            {
                var result = await _productsRepository.EditProductById(productId, productCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteById(Guid productId)
        {
            try
            {
                await _productsRepository.DeleteProductById(productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{productId}/IncreaseStock")]
        public async Task<IActionResult> IncreaseStockPerProduct(Guid productId, long quantity)
        {
            try
            {
                var result = await _productsRepository.IncreaseStockPerProduct(productId, quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
