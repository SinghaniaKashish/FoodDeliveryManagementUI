using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly FoodDeliveryDbContext _context;

        public MenuController(IMenuService menuService, FoodDeliveryDbContext context)
        {
            _menuService = menuService;
            _context = context;
        }

        // GET by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _menuService.GetMenuItemById(id);
            if (menuItem == null)
                return NotFound("Menu item not found.");
            return Ok(menuItem);
        }


        // GET
        [HttpGet("restaurant/{restaurantId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuByRestaurantId(int restaurantId)
        {
            var menuItems = await _menuService.GetMenuByRestaurantId(restaurantId);
            return Ok(menuItems);
        }


        // POST: api/Menu
        [HttpPost]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> AddMenuItem([FromBody] MenuItem menuItem)
        {
            
            var result = await _menuService.AddMenuItem(menuItem);
            if (result)
                return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.ItemId }, menuItem);

            return BadRequest("Failed to add menu item.");
        }



        // PUT
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuItem)
        {
            var result = await _menuService.UpdateMenuItem(
                id,
                menuItem.Name,
                menuItem.Category,
                menuItem.Price,
                menuItem.CuisineType,
                menuItem.Availability,
                menuItem.IsVeg,
                menuItem.ImagePath,
                menuItem.Ingredients.ToList()); 
            if (result)
                return NoContent();

            return BadRequest("Failed to update menu item.");
        }



        // DELETE by id
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuService.DeleteMenuItem(id);
            if (result)
                return NoContent();

            return BadRequest("Failed to delete menu item.");
        }



        [HttpPost("bulk-add")]
        public async Task<IActionResult> BulkAddMenuItems([FromBody] List<MenuItem> menuItems)
        {
            if (menuItems == null || !menuItems.Any())
            {
                return BadRequest("Menu items list cannot be empty.");
            }

            try
            {
                await _menuService.BulkMenuItemAdd(menuItems);
                return Ok(new { message = "Menu items added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding menu items.", error = ex.Message });
            }
        }

        // GET: api/Menu?cuisineTypes=Indian,Chinese
        [HttpGet("cuisine")]
        public IActionResult GetMenuItems([FromQuery] string cuisineTypes)
        {
            try
            {
                var cuisines = cuisineTypes?.Split(',').Select(c => c.Trim()).ToList();

                var menuItems = _context.MenuItems
                    .Where(item => cuisines.Contains(item.CuisineType))
                    .ToList();

                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }


        


    }
}
