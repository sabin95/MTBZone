using CatalogAPI.Commands;
using CatalogAPI.Repository;
using CatalogAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
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

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCommand categoryCommand)
        {
            try
            {
                var result = await _categoryRepository.AddCategory(categoryCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategoryById([FromRoute] Guid id,[FromBody] CategoryCommand categoryCommand)
        {
            try
            {
                var result = await _categoryRepository.EditCategoryById(id, categoryCommand);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById ([FromRoute] Guid id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
