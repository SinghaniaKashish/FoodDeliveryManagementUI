using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly FoodDeliveryDbContext _context;

        public OrderDetailsController(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Create OrderDetail
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetail orderDetail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetail.OrderDetailId }, orderDetail);
        }

        // Get OrderDetail by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
                return NotFound();

            return Ok(orderDetail);
        }

        // Update OrderDetail
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
                return BadRequest("OrderDetail ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete OrderDetail
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
                return NotFound();

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
   
