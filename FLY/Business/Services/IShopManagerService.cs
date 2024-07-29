using FLY.Business.Models.Customer;
using FLY.Business.Models.Order;
using FLY.Business.Models.OrderDetail;
using FLY.Business.Models.Product;
using FLY.Business.Models.Shop;
using FLY.Business.Models.VoucherOfShop;

namespace FLY.Business.Services
{
    public interface IShopManagerService
    {
        // Manager shop
        Task<ShopResponse> GetShopByIdAsync(int id);
        Task<bool>  UpdateShopInformation(ShopRequest request);
        // Manager Product
        Task<List<ProductResponse>> GetAllProductsAsync(int ShopId);
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<bool> UpdateProductInformation(ProductRequest request);
        Task<bool> DeleteProductInformation(ProductRequest request);
        Task<bool> CreateProductInformation(ProductResponse response);
        // Manager Order
        Task<List<OrderResponse>> GetAllOrdersAsync(int ShopId);
        Task<bool> UpdateOrderInformation(OrderRequest request);
        Task<List<OrderDetailResponse>> GetOrderDetailAsync(int OrderId);

        // Manager Revenue
        Task<float> GetRevenueAsync(int shopId, int month, int year);

        // Manager Voucher
        Task<List<VoucherOfShopResponse>> GetAllVouchersAsync(int ShopId);
        Task<VoucherOfShopResponse> GetVoucherByIdAsync(int VoucherId);
        Task<bool> UpdateVouchersInformation(VoucherOfShopRequest request);
        Task<bool> CreateVouchersInformation(VoucherOfShopResponse response);
        Task<bool> DeleteVouchersInformation(VoucherOfShopRequest request);

    }
}
