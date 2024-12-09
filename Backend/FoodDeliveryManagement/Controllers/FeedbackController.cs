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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly FoodDeliveryDbContext _context;

        public FeedbackController(IFeedbackService feedbackService, FoodDeliveryDbContext context)
        {
            _feedbackService = feedbackService;
            _context = context;
        }

        // GET: by id restaurant
        [HttpGet("restaurant/{restaurantId:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetFeedbackByRestaurantId(int restaurantId)
        {
            var feedbacks = await _feedbackService.GetFeedbackByRestaurantId(restaurantId);
            return Ok(feedbacks);
        }


        //feedback by driver id
        [HttpGet("driver/{driverId:int}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> GetFeedbackByDriverId(int driverId)
        {
            var feedbacks = await _context.Feedbacks.FirstOrDefaultAsync(f => f.DriverId == driverId);
            return Ok(feedbacks);
        }



        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null || feedback.OrderId == 0)
            {
                return BadRequest(new { message = "Invalid feedback data." });
            }

            var orderData = await _context.Orders
                .Where(o => o.OrderId == feedback.OrderId)
                .Join(
                    _context.Deliveries,
                    order => order.OrderId,
                    delivery => delivery.OrderId,
                    (order, delivery) => new
                    {
                        order.RestaurantId,
                        delivery.DriverId
                    }
                )
                .FirstOrDefaultAsync();

            if (orderData == null)
            {
                return NotFound(new {message = "Order or delivery details not found." });
            }

            feedback.RestaurantId = orderData.RestaurantId;
            feedback.DriverId = orderData.DriverId;
            feedback.FeedbackDate = DateTime.UtcNow;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Feedback submitted successfully!" });
        }


        // DELETE feedback
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var result = await _feedbackService.DeleteFeedback(id);
            if (result)
                return NoContent();

            return BadRequest("Failed to delete feedback.");
        }
    }
}
