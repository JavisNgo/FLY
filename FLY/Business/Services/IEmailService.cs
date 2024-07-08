using Google.Apis.Auth.OAuth2;

namespace FLY.Business.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string vertificationCode);
    }
}
