using Azure;
using FLY.Business.Models.Account;
using FLY.Business.Models.GoogleCloud;
using FLY.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FLY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/auth")]
        public async Task<IActionResult> Login([FromBody] AuthRequest loginInfo)
        {
            try
            {
                var account = await _authService.AuthenticateAccount(loginInfo);
                if (account != null)
                {
                    switch(account.Status)
                    {
                        case 0:
                            return BadRequest("Your account is locked by administrator");
                        case 1:
                            if(account.RoleId == 1)
                            {
                                bool check = await _authService.AuthenticateAccountAdvanced(account);
                                if (check)
                                {
                                    return Ok(new {message = "Email has been sent, please check email to verify your account, if you don't see it, check your spam" });
                                }
                                else
                                {
                                    return BadRequest("Something went wrong");
                                }
                            } 
                            else if(account.RoleId == 3 || account.RoleId == 2)
                            {
                                var token = await _authService.GenerateTokens(account.Email);
                                if (token.refreshToken.IsNullOrEmpty() || token.accessToken.IsNullOrEmpty())
                                {
                                    return BadRequest("Something went wrong");
                                }
                                return Ok(new { accessToken = token.accessToken, refreshToken = token.refreshToken });
                            }
                            break;
                        case 2:
                            await _authService.SendVerificationEmailForRegister(account.Email);
                            return Ok("Your account hasn't verified yet, please check email to verify your account, if you don't see it, check your spam");
                        default:
                            return BadRequest("Your status has not been configurated in system");
                    }
                }
                return NotFound("Account not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/auth/verify")]
        public async Task<IActionResult> VerifyLogin([FromBody] EmailVerificationView request)
        {
            try
            {
                var token = await _authService.GenerateTokens(request.Email, request.Code);
                if (token.accessToken.IsNullOrEmpty() || token.refreshToken.IsNullOrEmpty())
                {
                    return BadRequest("Something went wrong");
                }
                return Ok(new {accessToken = token.accessToken, refreshToken = token.refreshToken});
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/auth/refresh")]
        public async Task<IActionResult> AccessTokenRequest([FromBody] string refreshToken)
        {
            try
            {
                var token = await _authService.RefreshingAccessToken(refreshToken);
                if (token.accessToken.IsNullOrEmpty() || token.refreshToken.IsNullOrEmpty())
                {
                    return BadRequest("Something went wrong");
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequest request)
        {
            try
            {
                bool check = await _authService.RegisterCustomer(request);
                if(check)
                {
                    await _authService.SendVerificationEmailForRegister(request.Email);
                    return Ok("Create success, please check your email to verify your account, if you don't see it, check your spam");
                }
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/register/seller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterRequest request)
        {
            try
            {
                bool check = await _authService.RegisterSeller(request);
                if (check)
                {
                    await _authService.SendVerificationEmailForRegister(request.Email);
                    return Ok("Create success, please check your email to verify your account, if you don't see it, check your spam");
                }
                return BadRequest("Something went wrong");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/register/verify")]
        public async Task<IActionResult> VerifyRegister([FromBody] EmailVerificationView request)
        {
            try
            {
                bool check = await _authService.VerifyRegisterAccount(request);
                if(check)
                {
                    var token = await _authService.GenerateTokens(request.Email);
                    if (token.accessToken.IsNullOrEmpty() || token.refreshToken.IsNullOrEmpty())
                    {
                        return BadRequest("Something went wrong");
                    }
                    return Ok(new {token.accessToken, token.refreshToken});
                }
                return BadRequest("Invalid verification code, if you don't see it, check your spam");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/auth/forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            try
            {
                await _authService.SendRecoveringVerificationEmail(email);
                return Ok("Recover code sent, please check email to verify your account, if you don't see it, check your spam");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/auth/forget-password/verify")]
        public async Task<IActionResult> VerifyForgetAccount([FromBody] VerifyForgetPasswordRequest request)
        {
            try
            {
                var response = await _authService.VerifyRecoverAccount(request);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid verification code");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" Internal Server Error: " + ex.Message);
            }
        }
    }
}
