using FoodDeliveryManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantStatisticController : ControllerBase
    {
        private readonly FoodDeliveryDbContext _context;
        public RestaurantStatisticController(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        [HttpGet("{restaurantId}/statistics")]
        [Authorize(Roles = "Owner")]

        public IActionResult GetRestaurantStatistics(int restaurantId)
        {
            try
            {
                var totalRevenue = _context.Orders
                    .Where(o => o.RestaurantId == restaurantId)
                    .Sum(o => (decimal?)o.TotalAmount) ?? 0;

                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var totalRevenueThisMonth = _context.Orders
                    .Where(o => o.RestaurantId == restaurantId && o.OrderDate >= startOfMonth)
                    .Sum(o => (decimal?)o.TotalAmount) ?? 0;

                //best seller
                var bestSeller = _context.OrderDetails
                    .Where(od => _context.Orders.Any(o => o.RestaurantId == restaurantId && o.OrderId == od.OrderId))
                    .GroupBy(od => od.ItemId)
                    .Select(group => new
                    {
                        ItemId = group.Key,
                        TotalQuantity = group.Sum(od => od.Quantity)
                    })
                    .OrderByDescending(item => item.TotalQuantity)
                    .FirstOrDefault();

                var bestSellerItem = bestSeller != null
                    ? _context.MenuItems
                        .Where(mi => mi.ItemId == bestSeller.ItemId)
                        .Select(mi => new { mi.Name, bestSeller.TotalQuantity })
                        .FirstOrDefault()
                    : null;

                return Ok(new
                {
                    TotalRevenue = totalRevenue,
                    TotalRevenueThisMonth = totalRevenueThisMonth,
                    BestSeller = bestSellerItem
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching statistics.", error = ex.Message });
            }
        }

        [HttpGet("{ownerId}")]
        [Authorize(Roles = "Owner")]

        public IActionResult GetOwnerStatistics(int ownerId)
        {
            try
            {
                var restaurants = _context.Restaurants
                    .Where(r => r.UserId == ownerId)
                    .Select(r => new
                    {
                        r.Name,
                        TotalRevenue = _context.Orders
                            .Where(o => o.RestaurantId == r.RestaurantId)
                            .Sum(o => (decimal?)o.TotalAmount) ?? 0,
                        RevenueThisMonth = _context.Orders
                            .Where(o => o.RestaurantId == r.RestaurantId && o.OrderDate >= DateTime.Now.AddMonths(-1))
                            .Sum(o => (decimal?)o.TotalAmount) ?? 0,
                        BestSeller = _context.OrderDetails
                            .Where(od => _context.Orders.Any(o => o.RestaurantId == r.RestaurantId && o.OrderId == od.OrderId))
                            .GroupBy(od => od.ItemId)
                            .Select(group => new
                            {
                                ItemId = group.Key,
                                TotalQuantity = group.Sum(od => od.Quantity),
                                ItemName = _context.MenuItems
                                    .Where(mi => mi.ItemId == group.Key)
                                    .Select(mi => mi.Name)
                                    .FirstOrDefault()
                            })
                            .OrderByDescending(item => item.TotalQuantity)
                            .FirstOrDefault()
                    })
                    .ToList();

                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }


    }
}
