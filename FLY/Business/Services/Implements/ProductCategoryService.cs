using AutoMapper;
using FLY.Business.Models.Product;
using FLY.Business.Models.ProductCategory;
using FLY.DataAccess.Repositories;

namespace FLY.Business.Services.Implements
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<ProductCategoryResponse>> GetAllProductCategoriesAsync()
        {
            try
            {
                var ct = await _unitOfWork.ProductCategoryRepository.GetAsync();
                var result = _mapper.Map<List<ProductCategoryResponse>>(ct.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
