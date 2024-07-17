using FLY.Business.Models.Feedback;
using FLY.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace FLY.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpGet("GetAllFeedback/{shopId}")]
        public async Task<IActionResult> GetAllFeedbackOfShop(int shopId)
        {
            try
            {
                var getAll = await _service.GetAllFeedbackOfShop(shopId);
                if (getAll.IsNullOrEmpty())
                {
                    return Ok("Feedback is empty !!");
                }

                return Ok(getAll);
            }
            catch
            {
                return BadRequest("Valid");
            }

        }

        [HttpGet("GetOneFeedback/{shopId}/{accountId}")]
        public async Task<IActionResult> GetOneFb(int shopId, int accountId)
        {
            try
            {
                var getOne = await _service.GetOneFb(shopId, accountId);
                if (getOne == null)
                {
                    return Ok("Feedback not found !!");
                }

                return Ok(getOne);
            }
            catch
            {
                return BadRequest("Valid");
            }
        }

        [HttpPost("CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequest request)
        {
            try
            {
                var response = await _service.CreateFeedback(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateFeedback/{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] FeedbackRequest request)
        {
            try
            {
                var response = await _service.UpdateFeedback(feedbackId, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateStsAdmin/{feedbackId}")]
        public async Task<IActionResult> UpdateStsAdmin(int feedbackId, [FromBody] UpdateFeedbackRequest request)
        {
            try
            {
                var response = await _service.UpdateStsAdmin(feedbackId, request);
                if (response)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{feedbackId}/{accountId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId, int accountId)
        {
            try
            {
                var response = await _service.DeleteFeedback(feedbackId, accountId);
                if(response)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
