using FLY.Business.Exceptions;
using FLY.Business.Models.Blog;
using FLY.Business.Models.Product;
using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("/api/v1/blogs")]
        public async Task<IActionResult> GetBlogs()
        {
            try
            {
                var result = await _blogService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("/api/v1/blogs/blogName")]
        public async Task<IActionResult> GetBlogsByName([FromQuery] string blogName)
        {
            try
            {
                var result = await _blogService.GetBlogsByNameAsync(blogName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("/api/v1/blogs/{blogId}")]
        public async Task<IActionResult> GetBlogsByBlogId(int blogId)
        {
            try
            {
                var result = await _blogService.GetByBlogIdAsync(blogId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/api/v1/myBlogs/{accountId}")]
        public async Task<IActionResult> GetBlogsByAccountId(int accountId)
        {
            try
            {
                var result = await _blogService.GetByAccountIdAsync(accountId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("/api/v1/UBlog")]
        public async Task<IActionResult> UpdateBlog(BlogRequest request)
        {
            try
            {
                var result = await _blogService.UpdateBlog(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpPost("/api/v1/CBlog")]
        public async Task<IActionResult> CreateBlog(BlogResponse response)
        {
            try
            {
                var result = await _blogService.CreateBlog(response);
                if (result) return Ok("Create information success");
                else return StatusCode(500, "Something wrong when create product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpDelete("/api/v1/DBlog")]
        public async Task<IActionResult> DeleteBlog(BlogRequest request)
        {
            try
            {
                var result = await _blogService.DeleteBlog(request);
                if (result) return Ok("Delete information success");
                else return StatusCode(500, "Something wrong when delete product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

    }
}
