using FLY.Business.Models.Order;

namespace FLY.Business.Services
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
        Task<List<OrderHistoryResponse>> GetListOrderH(int accountId);

    }
}
