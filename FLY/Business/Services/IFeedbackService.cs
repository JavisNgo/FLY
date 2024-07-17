using FLY.Business.Models.Feedback;

namespace FLY.Business.Services
{
    public interface IFeedbackService
    {
        Task<IList<FeedbackResponse>> GetAllFeedbackOfShop(int shopId);
        Task<FeedbackResponse> CreateFeedback(FeedbackRequest request);
        Task<FeedbackResponse> GetOneFb(int shopId, int accountId);
        Task<FeedbackResponse> UpdateFeedback(int feedbackId, FeedbackRequest request);
        Task<bool> DeleteFeedback(int feedbackId, int accountId);
        Task<bool> UpdateStsAdmin(int feedbackId, UpdateFeedbackRequest request);
    }
}
