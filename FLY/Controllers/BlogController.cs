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

        [HttpGet]
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
    }
}
