using FoodDeliveryManagement.Data;
using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly FoodDeliveryDbContext _context;

        public DeliveryRepository(FoodDeliveryDbContext context)
        {
            _context = context;
        }

        // Get a delivery by  ID
        public async Task<Delivery> GetById(int deliveryId)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"Delivery with ID {deliveryId} not found.");
            return delivery;
        }

        // Get all deliveries assigned to a specific driver
        public async Task<IEnumerable<Delivery>> GetByDriverId(int driverId)
        {
            var deliveries = await _context.Deliveries
                                   .Where(d => d.DriverId == driverId).ToListAsync();
                                           
            return deliveries;
        }


        
    }
}
