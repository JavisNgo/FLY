using FLY.Business.Models.Blog;
using FLY.Business.Models.Product;
using FLY.Business.Models.Rating;
using FLY.Business.Models.VoucherOfShop;

namespace FLY.Business.Services
{
    public interface IBlogService
    {
        Task<List<BlogResponse>> GetAllAsync();
        Task<List<BlogResponse>> GetByAccountIdAsync(int accountId);
        Task<BlogResponse> GetByBlogIdAsync(int blogId);
        Task<List<BlogResponse>> GetBlogsByNameAsync(string name);
        Task<bool> CreateBlog(CreateBlogResponse respone);
        Task<bool> UpdateBlog(UpdateBlogRequest request);
        Task<bool> DeleteBlog(UpdateBlogRequest request);
    }
}