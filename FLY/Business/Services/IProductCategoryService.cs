using FLY.Business.Models.Product;
using FLY.Business.Models.ProductCategory;

namespace FLY.Business.Services
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryResponse>> GetAllProductCategoriesAsync();

    }
}
