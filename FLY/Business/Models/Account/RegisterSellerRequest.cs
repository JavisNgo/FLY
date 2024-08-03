using System.ComponentModel.DataAnnotations;

namespace FLY.Business.Models.Account
{
    public class RegisterSellerRequest : RegisterRequest
    {
        public int CitizenIdentification { get; set; }

        public int TaxCode { get; set; }
    }
}
