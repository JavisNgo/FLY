using FLY.Business.Models.Product;
using FLY.Business.Models.Shop;

namespace FLY.Business.Models.OrderDetail
{
    public class OrderHistoryByShopResponse
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int OrderQuantity { get; set; }

        public float ProductPrice { get; set; }
        public ShopResponse Shop { get; set; }

        public ProductResponse Product { get; set; }

    }
}
