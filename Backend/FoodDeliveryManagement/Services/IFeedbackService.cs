using FoodDeliveryManagement.Models;

namespace FoodDeliveryManagement.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetFeedbackByRestaurantId(int restaurantId);
        Task<bool> AddFeedback(Feedback feedback);
        Task<bool> DeleteFeedback(int feedbackId);

    }

}

