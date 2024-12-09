using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        // Get a delivery by ID
        public async Task<Delivery> GetDeliveryById(int deliveryId)
        {
            return await _deliveryRepository.GetById(deliveryId);
        }

        // Get all deliveries assigned to a specific driver
        public async Task<IEnumerable<Delivery>> GetDeliveriesByDriverId(int driverId)
        {
            return await _deliveryRepository.GetByDriverId(driverId);
        }

        
    }
}
