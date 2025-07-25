using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Services;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)

        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CreateUserdto> users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete([FromRoute] int id)
        {
            bool deleted = await _userService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserdto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var UserEntity = await _userService.AddAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = UserEntity.Id }, UserEntity);
        }


        [HttpPut]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateUserdto user)
        {
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _userService.UpdateAsync(Id, user);

            if (!isUpdated) return BadRequest();

            return Ok();
        }

    }
}
