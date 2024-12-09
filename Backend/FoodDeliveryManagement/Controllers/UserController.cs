using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound("User not found.");
            return Ok(user);
        }

        // GET
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var users = await _userService.GetUsersByRole(role);
            return Ok(users);
        }
       
        // PUT: api/User
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _userService.UpdateUser(user);
            if (result)
                return NoContent();
            return BadRequest("Failed to update user.");
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result)
                return NoContent();

            return BadRequest("Failed to delete user.");
        }
    }
}
