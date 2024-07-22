using FLY.Business.Models.OrderDetail;

namespace FLY.Business.Models.Order
{
    public class CreateOrderRequest
    {
        public int AccountId { get; set; }
        public List<OrderDetailRequest> OrderDetailRequests { get; set; }
        public float TotalPrice { get; set; }
    }
}
