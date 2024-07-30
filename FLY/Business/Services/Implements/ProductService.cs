using AutoMapper;
using FLY.Business.Models.Product;
using FLY.Business.Models.Shop;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using Microsoft.Extensions.Caching.Memory;

namespace FLY.Business.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private static readonly Random _random = new Random();

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(string sessionId, int? pageIndex, int? pageSize)
        {
            try
            {
                var cacheKey = $"Product_{sessionId}";
                if (!_cache.TryGetValue(cacheKey, out List<ProductResponse> shuffledItems))
                {
                    var productList = await _unitOfWork.ProductRepository
                        .GetAsync(p => p.Status == 1, null, "ProductCategory", pageIndex, pageSize);
                    shuffledItems = _mapper.Map<List<ProductResponse>>(productList.ToList()
                        .OrderBy(x => _random.Next()));
                    _cache.Set(cacheKey, shuffledItems, TimeSpan.FromMinutes(5));
                } else if (pageIndex.HasValue && pageSize.HasValue)
                {
                    return shuffledItems.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value).ToList();
                }
                
                return shuffledItems;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(id);
                var result = _mapper.Map<ProductResponse>(product);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductResponse>> GetProductsByCategoryAsync(string categoryName, 
            int pageIndex, int pageSize)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository
                    .GetAsync(p => p.ProductCategory.ProductCategoryName == categoryName && p.Status == 1, 
                        null, "ProductCategory", pageIndex, pageSize);
                return _mapper.Map<List<ProductResponse>>(products.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductResponse>> GetProductsByNameAsync(string name, 
            int pageIndex, int pageSize)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductName.Contains(name) && p.Status == 1,
                    null, "ProductCategory", pageIndex, pageSize);
                return _mapper.Map<List<ProductResponse>>(products.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetTotalPage(int pageSize)
        {
            try
            {
                int total;
                total = await _unitOfWork.ProductRepository.CountAsync();
                return (int)Math.Ceiling(total / (double)pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
