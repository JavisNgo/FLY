using FLY.Business.Exceptions;
using FLY.Business.Models.Order;
using FLY.Business.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var response = await _service.CreateOrder(request);
                return Ok(response);
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.statusCode, ex.Message);
            }
        }
    }
}
