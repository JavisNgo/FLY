using FLY.Business.Models.Account;

namespace FLY.Business.Models.Order
{
    public class OrderHistoryResponse
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalPrice { get; set; }
        public AccountResponse Account { get; set; }

    }
}
