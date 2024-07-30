using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemoryCache _cache;

        public ProductController(IProductService productService, IMemoryCache cache)
        {
            _productService = productService;
            _cache = cache;
        }
        [HttpPost("/api/v1/products/category/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery]string categoryName, 
            [FromQuery]int pageIndex = 10, [FromQuery]int pageSize = 1)
        {
            try
            {
                var result = await _productService.GetProductsByCategoryAsync(categoryName, pageIndex, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("/api/v1/products")]
        public async Task<IActionResult> GetProductsByName([FromQuery]string productName,
            [FromQuery] int pageIndex = 10, [FromQuery] int pageSize = 1)
        {
            try
            {
                var result = await _productService.GetProductsByNameAsync(productName, pageIndex, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("/api/v1/products")]
        public async Task<IActionResult> GetProducts([FromQuery] int? pageIndex = null, 
            [FromQuery] int? pageSize = null)
        {
            try
            {
                string sessionId;

                if (HttpContext.Session.GetString("SessionId") == null)
                {
                    sessionId = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("SessionId", sessionId);

                    var activeSessionIds = _cache.Get<List<string>>("ActiveSessions") ?? new List<string>();
                    if (!activeSessionIds.Contains(sessionId))
                    {
                        activeSessionIds.Add(sessionId);
                        _cache.Set("ActiveSessions", activeSessionIds, TimeSpan.FromMinutes(30));
                    }
                }
                else
                {
                    sessionId = HttpContext.Session.GetString("SessionId");
                }
                var result = await _productService.GetAllProductsAsync(sessionId, pageIndex, pageSize);
                if (!result.Any()) return NotFound("No products exist");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
