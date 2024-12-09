using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Repositories
{
    public interface IFeedbackRepository
    {
        public Task<IEnumerable<Feedback>> GetByRestaurantId(int restaurantId);
        public Task<bool> Add(Feedback feedback);
        public Task<bool> Delete(int feedbackId);
    }
}
