using FLY.Business.Exceptions;
using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {

        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet("/api/v1/AllProductCategories")]
        public async Task<IActionResult> GetAllProductCategoriesAsync()
        {
            try
            {
                var result = await _productCategoryService.GetAllProductCategoriesAsync();
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

    }
}
