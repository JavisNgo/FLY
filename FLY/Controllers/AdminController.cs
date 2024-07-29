using FLY.Business.Exceptions;
using FLY.Business.Models.Shop;
using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IShopManagerService _shopManagerService;
        private readonly IShopService _shopService;
        private readonly IAdminService _adminService;

        public AdminController(IShopManagerService shopManagerService, IShopService shopService, IAdminService adminService)
        {
            _shopManagerService = shopManagerService;
            _shopService = shopService;
            _adminService = adminService;
        }
        //Manager 
        [HttpGet("/api/v1/AllShop")]
        public async Task<IActionResult> GetAllShop()
        {
            try
            {
                var result = await _shopService.GetAllShopsAsync();
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }


        [HttpGet("/api/v1/AdminRevenue")]
        public async Task<IActionResult> GetAdminRevenueAsync( int month, int year)
        {
            try
            {
                var result = await _adminService.GetRevenueAdminAsync(month, year);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get order");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }


        [HttpGet("/api/v1/ShopDetail")]
        public async Task<IActionResult> GetShopByShopId(int shopId)
        {
            try
            {
                var result = await _shopManagerService.GetShopByIdAsync(shopId);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }


        [HttpPost("/api/v1/UShopStatus")]
        public async Task<IActionResult> UpdateShopStatus(ShopRequest request)
        {
            try
            {
                var result = await _adminService.UpdateShopStatus(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
    }
}
