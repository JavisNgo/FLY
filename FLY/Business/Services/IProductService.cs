using FLY.Business.Models.Product;

namespace FLY.Business.Services
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsAsync(string sessionId, int? pageIndex, int? pageSize);
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<List<ProductResponse>> GetProductsByCategoryAsync(string categoryName, int pageIndex, int pageSize);
        Task<List<ProductResponse>> GetProductsByNameAsync(string name, int pageIndex, int pageSize);
    }
}