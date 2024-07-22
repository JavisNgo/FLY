using FLY.Business.Exceptions;
using FLY.Business.Models.Customer;
using FLY.Business.Models.Order;
using FLY.Business.Models.Product;
using FLY.Business.Models.Shop;
using FLY.Business.Models.VoucherOfShop;
using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopManagerController : ControllerBase
    {
        private readonly IShopManagerService _shopManagerService;

        public ShopManagerController(IShopManagerService shopManagerService)
        {
            _shopManagerService = shopManagerService;
        }
        //Manager shop account
        [HttpGet("/api/v1/shopAccount")]
        public async Task<IActionResult> GetShopByShopId(int shopId)
        {
            try
            {
                var result = await _shopManagerService.GetShopByIdAsync(shopId);
                if (result!=null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }


        [HttpPost("/api/v1/UShopAccount")]
        public async Task<IActionResult> UpdateShopInformation(ShopRequest request)
        {
            try
            {
                var result = await _shopManagerService.UpdateShopInformation(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        //Manager Product
        [HttpGet("/api/v1/ShopProduct")]
        public async Task<IActionResult> GetAllProductByShopId(int shopId)
        {
            try
            {
                var result = await _shopManagerService.GetAllProductsAsync(shopId);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpGet("/api/v1/ShopProductOne")]
        public async Task<IActionResult> GetProductByShopId(int Id)
        {
            try
            {
                var result = await _shopManagerService.GetProductByIdAsync(Id);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        [HttpPut("/api/v1/UShopProduct")]
        public async Task<IActionResult> UpdateProductInformation(ProductRequest request)
        {
            try
            {
                var result = await _shopManagerService.UpdateProductInformation(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpPost("/api/v1/CShopProduct")]
        public async Task<IActionResult> CreateProductInformation(ProductResponse response)
        {
            try
            {
                var result = await _shopManagerService.CreateProductInformation(response);
                if (result) return Ok("Create information success");
                else return StatusCode(500, "Something wrong when create product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpDelete("/api/v1/DShopProduct")]
        public async Task<IActionResult> DeleteProductInformation(ProductRequest request)
        {
            try
            {
                var result = await _shopManagerService.DeleteProductInformation(request);
                if (result) return Ok("Delete information success");
                else return StatusCode(500, "Something wrong when delete product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        // Manager Order GetOrderDetailAsync
        //[HttpGet("/api/v1/LShopOrder")]
        //public async Task<IActionResult> GetAllOrders(int shopId)
        //{
        //    try
        //    {
        //        var result = await _shopManagerService.GetAllOrdersAsync(shopId);
        //        if (result != null) return Ok(result);
        //        else return StatusCode(500, "Something wrong when get order");
        //    }
        //    catch (ApiException ex)
        //    {
        //        return StatusCode(((int)ex.statusCode), ex.Message);
        //    }
        //}

        [HttpGet("/api/v1/ShopOrderDetail")]
        public async Task<IActionResult> GetOrderDetail(int OrderId)
        {
            try
            {
                var result = await _shopManagerService.GetOrderDetailAsync(OrderId);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get order detail");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        [HttpPut("/api/v1/UShopOrder")]
        public async Task<IActionResult> UpdateOrderInformation(OrderRequest request)
        {
            try
            {
                var result = await _shopManagerService.UpdateOrderInformation(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        // Manager Revenue

        //[HttpGet("/api/v1/ShopRevenue")]
        //public async Task<IActionResult> GetRevenueAsync(int shopId, int month, int year)
        //{
        //    try
        //    {
        //        var result = await _shopManagerService.GetRevenueAsync(shopId,month,year);
        //        if (result != null) return Ok(result);
        //        else return StatusCode(500, "Something wrong when get order");
        //    }
        //    catch (ApiException ex)
        //    {
        //        return StatusCode(((int)ex.statusCode), ex.Message);
        //    }
        //}
        // Manager Voucher

        [HttpGet("/api/v1/ShopVoucher")]
        public async Task<IActionResult> GetAllVouchers(int shopId)
        {
            try
            {
                var result = await _shopManagerService.GetAllVouchersAsync(shopId);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop voucher");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpGet("/api/v1/ShopVoucherOne")]
        public async Task<IActionResult> GetVoucherById(int Id)
        {
            try
            {
                var result = await _shopManagerService.GetVoucherByIdAsync(Id);
                if (result != null) return Ok(result);
                else return StatusCode(500, "Something wrong when get shop voucher");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        [HttpPut("/api/v1/UShopVoucher")]
        public async Task<IActionResult> UpdateVouchersInformation(VoucherOfShopRequest request)
        {
            try
            {
                var result = await _shopManagerService.UpdateVouchersInformation(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update voucher");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpPost("/api/v1/CShopVoucher")]
        public async Task<IActionResult> CreateVouchersInformation(VoucherOfShopResponse response)
        {
            try
            {
                var result = await _shopManagerService.CreateVouchersInformation(response);
                if (result) return Ok("Create information success");
                else return StatusCode(500, "Something wrong when create voucher");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }

        [HttpDelete("/api/v1/DShopVoucher")]
        public async Task<IActionResult> DeleteVouchersInformation(VoucherOfShopRequest request)
        {
            try
            {
                var result = await _shopManagerService.DeleteVouchersInformation(request);
                if (result) return Ok("Delete information success");
                else return StatusCode(500, "Something wrong when Delete voucher");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
    }
}
