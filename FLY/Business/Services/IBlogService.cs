using FLY.Business.Models.Blog;

namespace FLY.Business.Services
{
    public interface IBlogService
    {
        Task<List<BlogResponse>> GetAllAsync();
        Task<List<BlogResponse>> GetByAccountIdAsync(int accountId);
    }
}