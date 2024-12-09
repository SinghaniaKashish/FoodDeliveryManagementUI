using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // GET by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantById(id);
            if (restaurant == null)
                return NotFound("Restaurant not found.");
            return Ok(restaurant);
        }

        // GETALL
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurant();
            return Ok(restaurants);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> AddRestaurant([FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _restaurantService.AddRestaurant(restaurant);
            if (result)
                return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.RestaurantId }, restaurant);

            return BadRequest("Failed to add the restaurant.");
        }


        // PUT
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] Restaurant restaurant)
        {
            var result = await _restaurantService.UpdateRestaurant(restaurant);
            if (result)
                return NoContent();

            return BadRequest(new { message = "Failed to update the restaurant." });
        }


        // DELETE by id
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var result = await _restaurantService.DeleteRestaurant(id);
            if (result)
                return NoContent();

            return BadRequest("Failed to delete the restaurant.");
        }

        // SEARCH by id
        [HttpGet("search")]
        public async Task<IActionResult> SearchRestaurants([FromQuery] string? keyword, [FromQuery] string? cuisineType)
        {
            var restaurants = await _restaurantService.SearchRestaurant(keyword, cuisineType);
            return Ok(restaurants);
        }

        //get by owner id
        [HttpGet("ownerId")]
        public async Task<IActionResult> GetRestaurantByOwnerId(int ownerId)
        {
            var restaurants = await _restaurantService.GetRestaurantByOwnerId(ownerId);
            return Ok(restaurants);
        }
    }
}
