using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IDeliveryService
    {
        public Task<Delivery> GetDeliveryById(int deliveryId);
        public Task<IEnumerable<Delivery>> GetDeliveriesByDriverId(int driverId);
        
    }
}
