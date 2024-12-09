using FoodDeliveryManagement.Models;
using FoodDeliveryManagement.Repositories;

namespace FoodDeliveryManagement.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // Get feedback by restaurant ID
        public async Task<IEnumerable<Feedback>> GetFeedbackByRestaurantId(int restaurantId)
        {
            return await _feedbackRepository.GetByRestaurantId(restaurantId);
        }

        // Add new feedback
        public async Task<bool> AddFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException(nameof(feedback), "Feedback object cannot be null.");

            return await _feedbackRepository.Add(feedback);
        }

        // Delete feedback by ID
        public async Task<bool> DeleteFeedback(int feedbackId)
        {
            return await _feedbackRepository.Delete(feedbackId);
        }

    }

}
