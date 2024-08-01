using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FLY.Business.Models.Account
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }

        public string? Phone { get; set; }


        public string? Address { get; set; }

        public DateOnly? Dob { get; set; }
        public string Email { get; set; }

    }
}
