using FLY.Business.Models.Rating;
using FLY.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FLY.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _service;
        public RatingController(IRatingService service)
        {
            _service = service;
        }

        [HttpGet("GetRate/{shopId}/{accountId}")]
        public async Task<IActionResult> GetRatingShop(int shopId, int accountId)
        {
            try
            {
                var getOneRatingShop = await _service.GetRatingShop(shopId, accountId);
                if(getOneRatingShop == null)
                {
                    return NotFound();
                }
                return Ok(getOneRatingShop);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateRating")]
        public async Task<IActionResult> CreateRate([FromBody] RatingRequest request)
        {
            try
            {
                var response = await _service.CreateRate(request);
                if (response == null)
                {
                    return BadRequest("have some thing wrong !");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateRating")]
        public async Task<IActionResult> UpdateRating([FromBody] RatingRequest request)
        {
            try
            {
                var response = await _service.UpdateRate(request);
                if (response == null)
                {
                    return BadRequest("have some thing wrong !");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
