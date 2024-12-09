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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly FoodDeliveryDbContext _context;

        public OrderController(IOrderService orderService ,FoodDeliveryDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        // GET by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
                return NotFound("Order not found.");
            return Ok(order);
        }

        // GET by customer id
        [HttpGet("customer/{userId}")]
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> GetOrderByUserId(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == userId)  
                .Select(o => new
                {
                    o.OrderId,
                    o.Status,
                    o.DeliveryAddress,
                    o.TotalAmount,
                    o.OrderDate,
                    OrderDetails = o.OrderDetails.Select(od => new
                    {
                        od.OrderDetailId,
                        od.MenuItem.Name,
                        od.Quantity,
                        od.Price,
                        TotalPrice = od.Quantity * od.Price
                    }).ToList()
                })
                .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                return NotFound(new { message = "No orders found for this user." });
            }

            return Ok(orders);
        }


        // GET by restauranrt id
        [HttpGet("restaurant/{restaurantId:int}")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> GetOrdersByRestaurantId(int restaurantId)
        {
            var orders = await _orderService.GetOrdersByRestaurantId(restaurantId);
            return Ok(orders);
        }

      
        //place order
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var firstMenuItemId = order.OrderDetails.FirstOrDefault()?.ItemId;
                if (firstMenuItemId == null)
                    return BadRequest(new {message = "No items in the order." });

                var restaurantId = await _context.MenuItems
                    .Where(mi => mi.ItemId == firstMenuItemId)
                    .Select(mi => mi.RestaurantId)
                    .FirstOrDefaultAsync();

                if (restaurantId == 0)
                    return NotFound(new {message = "Restaurant not found for the given menu item." });

                order.RestaurantId = restaurantId;

                _context.Orders.Add(order);

                var orderId = order.OrderId;

                foreach (var detail in order.OrderDetails)
                {
                    detail.OrderDetailId = 0;
                    detail.OrderId = orderId;
                    detail.CalculateTotalPrice();
                    _context.OrderDetails.Add(detail);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { orderId = orderId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { message = $"Internal server error: {innerException}" });
            }
        }


        //update status for owner
        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var validStatuses = new[] { "Accepted", "Prepared" };

            if (!validStatuses.Contains(request.NewStatus))
            {
                return BadRequest(new { message = "Invalid status. Allowed values are 'Accepted' or 'Prepared'." });
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            order.Status = request.NewStatus;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Order status updated to '{request.NewStatus}'." });
        }


        //asign driver
        [HttpPost("deliveries")]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> AssignDriverToOrder([FromBody] AssignDriverDto dto)
        {
            var order = await _context.Orders.FindAsync(dto.OrderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            if (order.Status != "Prepared")
            {
                return BadRequest(new { message = "Driver can only be assigned to 'Prepared' orders." });
            }

            var driver = await _context.Users
                .Where(u => u.Role == "Driver" && u.IsActive)
                .FirstOrDefaultAsync(u => u.UserId == dto.DriverId);

            if (driver == null)
            {
                return NotFound(new { message = "Driver not found or unavailable." });
            }

            var delivery = new Delivery
            {
                OrderId = dto.OrderId,
                DriverId = dto.DriverId,
                DeliveryAddress = order.DeliveryAddress
            };

            _context.Deliveries.Add(delivery);
            order.Status = "OutForDelivery";
            driver.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Driver assigned and delivery created." });
        }


        //assign driver automatically
        [HttpPatch("{orderId}/assign-driver")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> AssignDriverToOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            // Check order is prepared
            if (order.Status != "Prepared")
            {
                return BadRequest(new { message = "Order must be in 'Prepared' status to assign a driver." });
            }

            var availableDrivers = await _context.Users
                .Where(u => u.Role == "Driver" && u.IsActive)
                .ToListAsync();

            if (availableDrivers.Count == 0)
            {
                return BadRequest(new { message = "No available drivers found." });
            }

            // Randomly select a driver
            var randomDriver = availableDrivers[new Random().Next(availableDrivers.Count)];

            var delivery = new Delivery
            {
                OrderId = orderId,
                DriverId = randomDriver.UserId,
                DeliveryTime = DateTime.UtcNow,
                DeliveryAddress = order.DeliveryAddress
            };

            _context.Deliveries.Add(delivery);
            order.Status = "OutForDelivery";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Driver assigned successfully.", delivery });
        }


        //update by driver
        [HttpPatch("deliveries/{deliveryId}/status")]
        [Authorize(Roles = "Driver")]

        public async Task<IActionResult> UpdateDeliveryStatus(int deliveryId)
        {
            var delivery = await _context.Deliveries
                                          .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);

            if (delivery == null)
            {
                return NotFound(new { message = "Delivery not found." });
            }

            var order = await _context.Orders
                                      .FirstOrDefaultAsync(o => o.OrderId == delivery.OrderId);

            if (order != null)
            {
                order.Status = "Delivered";
            }

            var driver = await _context.Users
                                       .FirstOrDefaultAsync(u => u.UserId == delivery.DriverId);

            if (driver == null)
            {
                return BadRequest(new { message = "Driver not found." });
            }

            driver.IsActive = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Delivery status and order status updated successfully." });
        }


        // cancel order
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrder(id);
            if (result)
                return NoContent();

            return BadRequest("Failed to cancel order.");
        }
    }
}
