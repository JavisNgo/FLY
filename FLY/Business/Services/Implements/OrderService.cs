using AutoMapper;
using FLY.Business.Exceptions;
using FLY.Business.Models.Order;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace FLY.Business.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {

            var response = new OrderResponse();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    float totalPriceOrder = 0;
                    int i = 0;
                    var existCart = await _unitOfWork.CartRepository
                                                .GetAsync(filter: x => x.AccountId == request.AccountId
                                                                    && x.Status == 0);
                    if (existCart.IsNullOrEmpty())
                    {
                        throw new ApiException(HttpStatusCode.BadRequest, "Cart is empty !!");
                    }

                    var order = _mapper.Map<Order>(request);
                    order.OrderDate = DateTime.Now;
                    order.Status = 1;
                    await _unitOfWork.OrderRepository.InsertAsync(order);
                    await Task.Delay(200);
                    await _unitOfWork.SaveAsync();

                    var getIEOrder = await _unitOfWork.OrderRepository
                                                        .GetAsync(filter: x => x.AccountId == request.AccountId
                                                                            && x.OrderDate == order.OrderDate);
                    var getOneOrder = getIEOrder.FirstOrDefault();


                    var orderDetails = _mapper.Map<List<OrderDetail>>(request.OrderDetailRequests);
                    foreach (var cart in existCart)
                    {

                        var exitsProduct = await _unitOfWork.ProductRepository.GetByIDAsync(cart.ProductId);

                        if (exitsProduct != null && exitsProduct.Status == 1)
                        {
                            if (cart.CartQuantity > exitsProduct.ProductQuatity)
                            {
                                throw new ApiException(HttpStatusCode.BadRequest, "Not enouge quantity for this product");
                            }
                        }

                        //Them OrderDetail
                        var orderDetail = _mapper.Map<OrderDetail>(orderDetails[i]);
                        orderDetail.OrderId = getOneOrder.OrderId;
                        orderDetail.OrderQuantity = cart.CartQuantity;
                        orderDetail.Status = 1;
                        await _unitOfWork.OrderDetailRepository.InsertAsync(orderDetail);
                        await Task.Delay(200);
                        await _unitOfWork.SaveAsync();
                        i++;

                        totalPriceOrder += cart.CartQuantity * orderDetails.Select(x => x.ProductPrice).FirstOrDefault();

                        orderDetails.Select(x => x.OrderQuantity = cart.CartQuantity);

                        order.TotalPrice = totalPriceOrder;
                        cart.Product.ProductQuatity -= 1;

                        if (cart.Product.ProductQuatity <= 0)
                        {
                            cart.Product.Status = 0;
                        }
                        await _unitOfWork.CartRepository.DeleteAsync(cart.CartId);
                        await Task.Delay(100);
                        await _unitOfWork.SaveAsync();
                    }
                    transaction.Commit();

                    response = _mapper.Map<OrderResponse>(order);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return response;
        }
    }
}
