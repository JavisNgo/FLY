using AutoMapper;
using FLY.Business.Models.Blog;
using FLY.DataAccess.Repositories;

namespace FLY.Business.Services.Implements
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BlogResponse>> GetAllAsync()
        {
            try
            {
                var blogs = await _unitOfWork.BlogRepository.GetAsync(includeProperties: "Account");
                var result = _mapper.Map<List<BlogResponse>>(blogs.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BlogResponse>> GetByAccountIdAsync(int accountId)
        {
            try
            {
                var blogs = await _unitOfWork.BlogRepository.GetAsync(filter: x => x.AccountId == accountId);
                var result = _mapper.Map<List<BlogResponse>>(blogs);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
