using FLY.Business.Exceptions;
using FLY.Business.Models.Account;
using FLY.Business.Models.Blog;
using FLY.Business.Models.Customer;
using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("/api/v1/user")]
        public async Task<IActionResult> UpdateCustomerInformation(UpdateInfoRequest request)
        {
            try
            {
                var result = await _customerService.UpdateCustomerInformation(request);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update account");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
        [HttpGet("/api/v1/user/{accountId}")]
        public async Task<IActionResult> GetByAccountId([FromQuery] int accountId)
        {
            try
            {
                var result = await _customerService.GetByAccountIdAsync(accountId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("/api/v1/UCustomer")]
        public async Task<IActionResult> UpdateAccount(CustomerResponse response)
        {
            try
            {
                var result = await _customerService.UpdateCustomer(response);
                if (result) return Ok("Update information success");
                else return StatusCode(500, "Something wrong when update product");
            }
            catch (ApiException ex)
            {
                return StatusCode(((int)ex.statusCode), ex.Message);
            }
        }
    }
}
