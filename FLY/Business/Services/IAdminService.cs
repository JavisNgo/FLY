using FLY.Business.Models.Shop;

namespace FLY.Business.Services
{
    public interface IAdminService
    {
        Task<bool> UpdateShopStatus(ShopRequest request);
        Task<bool> DeleteShop(ShopRequest request);
        Task<float> GetRevenueAdminAsync(int month, int year);

    }
}
