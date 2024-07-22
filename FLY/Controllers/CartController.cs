using FLY.Business.Exceptions;
using FLY.Business.Models.Carte;
using FLY.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FLY.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("GetAll/{customerId}")]
        public async Task<IActionResult> GetAll(int customerId)
        {
            try
            {
                var response = await _service.GetCartOfCustomer(customerId);
                if (response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.statusCode, ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProductToCart(CartRequest request)
        {
            try
            {
                var response = await _service.AddProductToCart(request);
                if(response == null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }catch (ApiException ex)
            {
                return StatusCode((int)ex.statusCode, ex.Message);
            }
        }
        [HttpPost("Sub")]
        public async Task<IActionResult> SubProductFromCart(SubCartRequest request)
        {
            try
            {
                var response = await _service.SubProductToCart(request);
                if (!response)
                {
                    return BadRequest();
                }
                return Ok("Success");
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.statusCode, ex.Message);
            }
        }
    }
}
