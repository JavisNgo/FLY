using FLY.Business.Models.Carte;

namespace FLY.Business.Services
{
    public interface ICartService
    {
        Task<CartResponse> GetCartOfCustomer(int accountId);
        Task<CartResponse> AddProductToCart(CartRequest request);
        Task<bool> SubProductToCart(SubCartRequest request);
    }
}
