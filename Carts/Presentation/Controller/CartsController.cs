using Microsoft.AspNetCore.Mvc;
using classLib.CartDtos;
using Carts.Service;
using Carts.Data;

namespace Carts.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        private readonly ICartService _CartService;

        public CartsController(ICartService CartService)

        {
            _CartService = CartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Cart> Carts = await _CartService.GetAllAsync();

            return Ok(Carts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var Cart = await _CartService.GetByIdAsync(id);

            if (Cart == null)
            {
                return NotFound();
            }
            return Ok(Cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            bool deleted = await _CartService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartDto Cart)
        {
            if (Cart == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var CartEntity = await _CartService.AddAsync(Cart);

            return CreatedAtAction(nameof(GetById), new { id = CartEntity.Id }, CartEntity);
        }


        [HttpPut]
        public async Task<IActionResult> Update(int Id, [FromBody] CartDto Cart)
        {
            if (Cart == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _CartService.UpdateAsync(Id, Cart);

            if (!isUpdated) return BadRequest();

            return Ok();
        }

        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetByUserId([FromRoute] int userId)
        //{
        //    var Cart = await _CartService.GetByUserIdAsync(userId);
        //    if (Cart == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Cart);

        //}

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartDto cartDto)
        {
            if (cartDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedCart = await _CartService.AddItemToCartAsync(cartDto);
            if (updatedCart == null)
            {
                return NotFound();
            }
            return Ok(updatedCart);
        }

    }
}

