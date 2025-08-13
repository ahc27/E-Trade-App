using Microsoft.AspNetCore.Mvc;
using CategoryMicroservice.Service;
using CategoryMicroservice.Service.Dtos;
using classLib;

namespace CategoryMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategorysController(ICategoryService categoryService)

        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<GetCategoryDto> categorys = await _categoryService.GetAllAsync();

            return Ok(categorys);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            bool deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var CategoryEntity = await _categoryService.AddAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = CategoryEntity.Id }, CategoryEntity);
        }


        [HttpPut]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateCategoryDto category)
        {
            if (category == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _categoryService.UpdateAsync(Id, category);

            if (!isUpdated) return BadRequest();

            return Ok();
        }

    }
}

