using FLY.Business.Models.ProductCategory;
using FLY.Business.Models.Shop;
using FLY.Business.Models.VoucherOfShop;
using FLY.DataAccess.Entities;

namespace FLY.Business.Models.Product
{
    public class ProductResponse
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

        public int Status { get; set; }
        public virtual ProductCategoryResponse ProductCategory { get; set; } = null!;

        public virtual ShopResponse Shop { get; set; } = null!;
        public virtual VoucherOfShopResponse Voucher { get; set; } = null!;
    }
}
