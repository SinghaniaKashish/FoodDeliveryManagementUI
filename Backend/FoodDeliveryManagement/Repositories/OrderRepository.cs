using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class OrderRepository :IOrderRepository
    {
        private readonly FoodDeliveryDbContext _context;
        public OrderRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }


        public async Task<Order> GetById(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            return order;
        }

        

        public async Task<IEnumerable<Order>> GetByRestaurantId(int restaurantId)
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails)
                                              .Where(o => o.RestaurantId == restaurantId)
                                              .ToListAsync();
            return orders;
        }

        public async Task<bool> Add(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }

     

        public async Task<bool> Cancel(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            if (order.Status.ToLower() == "cancelled")
                throw new InvalidOperationException("Order is already cancelled.");

            var timeSinceOrder = DateTime.UtcNow - order.OrderDate;
            if (timeSinceOrder.TotalMinutes > 2)
                throw new InvalidOperationException("Order cannot be cancelled as more than 2 minutes have passed since it was placed.");

            order.Status = "cancelled";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
