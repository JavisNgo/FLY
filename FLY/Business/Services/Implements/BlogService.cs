using AutoMapper;
using Azure;
using FLY.Business.Exceptions;
using FLY.Business.Models.Blog;
using FLY.Business.Models.Product;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.Identity.Client;
using System.Drawing.Printing;
using System.Net;

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

        public async Task<bool> CreateBlog(BlogResponse respone)
        {
            var pd = await _unitOfWork.BlogRepository.FindAsync(a => a.BlogId == respone.BlogId);
            var existedBl = pd.FirstOrDefault();

            if (existedBl != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Blog already exists");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Blog newBlog = _mapper.Map<Blog>(respone);
                    await _unitOfWork.BlogRepository.InsertAsync(newBlog);
                    await _unitOfWork.SaveAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteBlog(BlogRequest request)
        {
            var bl = await _unitOfWork.BlogRepository.FindAsync(a => a.BlogId == request.BlogId);
            var existedBl = bl.FirstOrDefault();
            if (existedBl == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your blog is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedBl, request);
                    await _unitOfWork.BlogRepository.DeleteAsync(existedBl);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
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

        public async Task<List<BlogResponse>> GetBlogsByNameAsync(string name)
        {
            try
            {
                var blogs = await _unitOfWork.BlogRepository.GetAsync(p => p.BlogName.Contains(name) && p.Status == 1,
                    null, "Account");
                return _mapper.Map<List<BlogResponse>>(blogs.ToList());
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
                var blogs = await _unitOfWork.BlogRepository.GetAsync(filter: x => x.AccountId == accountId,
                    includeProperties: "Account");
                var result = _mapper.Map<List<BlogResponse>>(blogs);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlogResponse> GetByBlogIdAsync(int blogId)
        {
            try
            {
                var blogs = await _unitOfWork.BlogRepository.GetByIDAsync(blogId);
                var result = _mapper.Map<BlogResponse>(blogs);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateBlog(BlogRequest request)
        {
            var bl = await _unitOfWork.BlogRepository.FindAsync(a => a.BlogId == request.BlogId);
            var existedBl = bl.FirstOrDefault();
            if (existedBl == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your Blog is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedBl, request);
                    await _unitOfWork.BlogRepository.UpdateAsync(existedBl);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
