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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly FoodDeliveryDbContext _context;

        public InventoryController(IInventoryService inventoryService, FoodDeliveryDbContext context)
        {
            _inventoryService = inventoryService;
            _context = context;
        }

        // GET:by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInventoryItem(int id)
        {
            var inventory = await _inventoryService.GetInventoryItem(id);
            if (inventory == null)
                return NotFound(new { message = "Inventory item not found." });
            return Ok(inventory);
        }


        // GET by restaurant id
        [HttpGet("restaurant/{restaurantId:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetInventoryByRestaurantId(int restaurantId)
        {
            var inventoryItems = await _inventoryService.GetInventoryByRestaurantId(restaurantId);
            return Ok(inventoryItems);
        }


        // POST: 
        [HttpPost]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> AddInventoryItem([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _inventoryService.AddInventoryItem(inventory);
            if (result)
                return Ok(new {message = "Inventory item added successfully." });

            return BadRequest(new { message= "Failed to add inventory item." });
        }



        // PUT deduct on order
        [HttpPost("deduct-ingredients/{orderId}")]
        public async Task<IActionResult> DeductIngredientsForOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.Status == "Prepared");

            if (order == null)
                return NotFound(new {message = "Order not found or is not in the 'Prepared' status." });

            foreach (var detail in order.OrderDetails)
            {
                var ingredientUsages = await _context.IngredientUsages
                    .Where(iu => iu.ItemId == detail.ItemId)
                    .ToListAsync();

                foreach (var usage in ingredientUsages)
                {
                    // Fetch the inventory item for the ingredient
                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i =>
                            i.RestaurantId == order.RestaurantId &&
                            i.IngredientName == usage.IngredientName);

                    if (inventory == null)
                        continue; 
                    inventory.Quantity -= (int)(usage.Quantity * detail.Quantity);
                    // Update inventory status
                    if (inventory.Quantity <= 0)
                    {
                        inventory.Quantity = 0;
                        inventory.Status = "Out Of Stock";
                    }
                    else if (inventory.Quantity <= inventory.ReorderLevel)
                    {
                        inventory.Status = "Low Stock";
                    }
                    else
                    {
                        inventory.Status = "In Stock";
                    }

                    _context.Inventories.Update(inventory);
                }
            }

            await _context.SaveChangesAsync();

            return Ok( new { message = "Inventory updated successfully after order preparation." });
        }


        // PUT by res id
        [HttpPut("add-quantity/{inventoryId:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AddQuantityToInventory(int inventoryId, [FromBody] int quantityToAdd)
        {
            if (quantityToAdd <= 0)
                return BadRequest(new { message = "Quantity to add must be greater than zero." });

            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null)
                return NotFound(new { message = "Inventory item not found." });

            inventory.Quantity += quantityToAdd;
            if (inventory.Quantity == 0)
                inventory.Status = "Out Of Stock";
            else if (inventory.Quantity <= inventory.ReorderLevel)
                inventory.Status = "Low Stock";
            else
                inventory.Status = "In Stock";

            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Quantity updated. Current Quantity: {inventory.Quantity}, Status: {inventory.Status}" });
        }


        // DELETE
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var result = await _inventoryService.DeleteInventoryItem(id);
            if (result)
                return NoContent();

            return BadRequest(new { message = "Failed to delete inventory item." });
        }
    }
}
