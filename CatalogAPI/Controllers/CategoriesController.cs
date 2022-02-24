using CatalogAPI.Common.Commands;
using CatalogAPI.Common.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoriesRepository.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            try
            {
                var category = await _categoriesRepository.GetCategoryById(categoryId);
                if (category is null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCommand categoryCommand)
        {
            try
            {
                var result = await _categoriesRepository.AddCategory(categoryCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> EditCategoryById([FromRoute] Guid categoryId, [FromBody] CategoryCommand categoryCommand)
        {
            try
            {
                var result = await _categoriesRepository.EditCategoryById(categoryId, categoryCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] Guid categoryId)
        {
            try
            {
                await _categoriesRepository.DeleteCategoryById(categoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
