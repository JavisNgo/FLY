using AutoMapper;
using FLY.Business.Exceptions;
using FLY.Business.Models.Carte;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace FLY.Business.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartResponse> AddProductToCart(CartRequest request)
        {
            var checkStsCart = await _unitOfWork.CartRepository
                                                .GetAsync(filter: x => x.AccountId == request.AccountId
                                                                    && x.Status == 0,
                                                          includeProperties: "Product,Account");

            var getOneProduct = checkStsCart.Where(x => x.ProductId == request.ProductId)
                                            .FirstOrDefault();

            
            var map = _mapper.Map<Cart>(request);
            map.Status = 0;

            if (getOneProduct != null)
            {
                getOneProduct.CartQuantity += 1;
                await _unitOfWork.SaveAsync();
                var responseEarly = _mapper.Map<CartResponse>(map);
                responseEarly.ProductInCarts = _mapper.Map<List<ProductInCartResponse>>(checkStsCart);
                return responseEarly;
            }


            await _unitOfWork.CartRepository.InsertAsync(map);
            await Task.Delay(300);
            await _unitOfWork.SaveAsync();

            var getlistProduct = await _unitOfWork.CartRepository
                                                .GetAsync(filter: x => x.AccountId == request.AccountId
                                                                    && x.Status == 0,
                                                          includeProperties: "Product,Account");

            var response = _mapper.Map<CartResponse>(map);
            response.ProductInCarts = _mapper.Map<List<ProductInCartResponse>>(getlistProduct);

            return response;
        }

        public async Task<CartResponse> GetCartOfCustomer(int accountId)
        {
            var getCartAsync = await _unitOfWork.CartRepository
                                                .GetAsync(filter: x => x.AccountId == accountId
                                                                    && x.Status == 0,
                                                          includeProperties: "Product,Account");
            var getCart = getCartAsync.FirstOrDefault();
            var response = _mapper.Map<CartResponse>(getCart);
            response.ProductInCarts = _mapper.Map<List<ProductInCartResponse>>(getCartAsync);

            return response;
        }

        public async Task<bool> SubProductToCart(SubCartRequest request)
        {
            var checkStsCart = await _unitOfWork.CartRepository
                                                .GetAsync(filter: x => x.AccountId == request.AccountId
                                                                    && x.ProductId == request.ProductId
                                                                    && x.Status == 0,
                                                          includeProperties: "Product,Account");

            var getOneProduct = checkStsCart.Where(x => x.ProductId == request.ProductId)
                                            .FirstOrDefault();

            var map = _mapper.Map<Cart>(request);
            map.Status = 0;

            
            if (getOneProduct != null)
            {
                getOneProduct.CartQuantity = (getOneProduct.CartQuantity >= 1) ? 
                                                getOneProduct.CartQuantity -= 1 
                                                : getOneProduct.CartQuantity = 0;

                if(getOneProduct.CartQuantity == 0)
                {
                    await _unitOfWork.CartRepository.DeleteAsync(getOneProduct.CartId);
                }
            }
            await Task.Delay(300);
            await _unitOfWork.SaveAsync();
            
            return true;
        }
    }
}
