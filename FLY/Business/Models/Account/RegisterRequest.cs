namespace FLY.Business.Models.Account
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateOnly Dob { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
