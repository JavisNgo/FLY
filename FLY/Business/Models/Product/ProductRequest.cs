namespace FLY.Business.Models.Product
{
    public class ProductRequest
    {
        public int ProductId { get; set; }

        public int ShopId { get; set; }

        public int? VoucherId { get; set; }

        public int ProductCategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public string ProductInfor { get; set; } = null!;

        public double ProductPrice { get; set; }

        public int ProductQuatity { get; set; }

        public string ImageProduct { get; set; } = null!;
    }
}
