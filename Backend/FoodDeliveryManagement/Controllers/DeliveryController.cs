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
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly FoodDeliveryDbContext _context;

        public DeliveryController(IDeliveryService deliveryService, FoodDeliveryDbContext context)
        {
            _deliveryService = deliveryService;
            _context = context;
        }

        // GET by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeliveryById(int id)
        {
            var delivery = await _deliveryService.GetDeliveryById(id);
            if (delivery == null)
                return NotFound("Delivery not found.");
            return Ok(delivery);
        }

        

        //get delivery by driver id
        [HttpGet("driver/{driverId}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> GetDeliveriesByDriver(int driverId)
        {
            var deliveries = await _context.Deliveries
                .Where(d => d.DriverId == driverId)
                .Join(
                    _context.Orders,
                    delivery => delivery.OrderId,
                    order => order.OrderId,
                    (delivery, order) => new
                    {
                        delivery.DeliveryId,
                        delivery.OrderId,
                        delivery.DeliveryAddress,
                        Totalamount = order.TotalAmount,
                        OrderStatus = order.Status 
                    }
                )
                .ToListAsync();

            if (!deliveries.Any())
            {
                return NotFound(new { message = "No deliveries found for the specified driver." });
            }

            return Ok(deliveries);
        }


    }
}
