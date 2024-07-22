namespace FLY.Business.Models.OrderDetail
{
    public class OrderDetailResponse
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int OrderQuantity { get; set; }

        public float ProductPrice { get; set; }
    }
}
