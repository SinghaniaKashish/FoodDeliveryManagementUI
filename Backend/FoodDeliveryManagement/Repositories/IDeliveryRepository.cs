using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IDeliveryRepository
    {
        public Task<Delivery> GetById(int deliveryId);
        public Task<IEnumerable<Delivery>> GetByDriverId(int driverId);

    }
}
