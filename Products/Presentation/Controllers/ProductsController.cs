using Microsoft.AspNetCore.Mvc;
using Products.Service;
using classLib.ProductDtos;

namespace Products.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _ProductService;

        public ProductsController(IProductService ProductService)

        {
            _ProductService = ProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDto> Products = await _ProductService.GetAllAsync();

            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var Product = await _ProductService.GetByIdAsync(id);

            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            bool deleted = await _ProductService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductDto Product)
        {
            if (Product == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ProductEntity = await _ProductService.AddAsync(Product);

            return CreatedAtAction(nameof(GetById), new { id = ProductEntity.Id }, ProductEntity);
        }


        [HttpPut]
        public async Task<IActionResult> Update(int Id, [FromBody] ProductDto Product)
        {
            if (Product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _ProductService.UpdateAsync(Id, Product);

            if (!isUpdated) return BadRequest();

            return Ok();
        }

    }
}

