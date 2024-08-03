using FLY.Business.Services;
using FLY.Business.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("/api/v1/shop/name")]
        public async Task<IActionResult> GetShopByName([FromQuery] string shopName)
        {
            try
            {
                var result = await _shopService.GetShopByNameAsync(shopName);
                if(!result.Any())
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/api/v1/shop")]
        public async Task<IActionResult> GetShopByAccountId()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token != null)
            {
                var accountId = ValidateJwtToken(token);
                if(accountId != null)
                {
                    var result = await _shopService.GetShopByAccountId(accountId.Value);
                    return Ok(result);
                }
                return Unauthorized("Not found account's information in token");
            }
            else
            {
                return Unauthorized("Please login account");
            }
        }

        private int? ValidateJwtToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var accountId = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            return accountId != null ? int.Parse(accountId) : (int?)null;
        }
    }
}
