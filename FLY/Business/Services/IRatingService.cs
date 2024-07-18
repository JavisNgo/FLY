using FLY.Business.Models.Rating;

namespace FLY.Business.Services
{
    public interface IRatingService
    {
        Task<RatingResponse> GetRatingShop(int shopId, int accountId);
        Task<RatingResponse> CreateRate(RatingRequest request);
        Task<RatingResponse> UpdateRate(RatingRequest request);
    }
}
