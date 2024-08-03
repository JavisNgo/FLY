using FLY.Business.Models.Account;
using FLY.Business.Models.GoogleCloud;

namespace FLY.Business.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAccount(AuthRequest authRequest);
        Task<bool> AuthenticateAccountAdvanced(AuthResponse authReponse);
        Task ChangeNewPassword(ChangeNewPasswordRequest request);
        Task<(string accessToken, string refreshToken)> GenerateTokens(string email, string verificationCode);
        Task<(string accessToken, string refreshToken)> GenerateTokens(string email);
        Task<(string accessToken, string refreshToken)> RefreshingAccessToken(string oldRefreshToken);
        Task<bool> RegisterCustomer(RegisterRequest registerCustomerRequest);
        Task<bool> RegisterSeller(RegisterSellerRequest registerCustomerRequest);
        Task SendVerificationEmailForRegister(string email);
        Task SendVerificationEmail(string email);
        Task<bool> VerifyAccount(string email, string verificationCode);
        Task<VerifyForgetPasswordRequest> VerifyRecoverAccount(VerifyForgetPasswordRequest request);
        Task<bool> VerifyRegisterAccount(EmailVerificationView emailVerificationView);
        Task SendRecoveringVerificationEmail(string email);
    }
}