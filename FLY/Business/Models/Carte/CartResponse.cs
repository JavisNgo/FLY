namespace FLY.Business.Models.Carte
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public string Email { get; set; }
        public List<ProductInCartResponse> ProductInCarts { get; set; }
    }

    public class ProductInCartResponse
    {
        public string ProductCategoryName { get; set; }
        public string ProductName { get; set; }
        public string ImageProduct { get; set; } = null!;
        public double ProductPrice { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int CartQuantity { get; set; }
    }
}
