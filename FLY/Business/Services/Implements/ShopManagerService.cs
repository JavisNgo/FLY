using AutoMapper;
using FLY.Business.Exceptions;
using FLY.Business.Models.Customer;
using FLY.Business.Models.Order;
using FLY.Business.Models.OrderDetail;
using FLY.Business.Models.Product;
using FLY.Business.Models.Shop;
using FLY.Business.Models.VoucherOfShop;
using FLY.DataAccess.Entities;
using FLY.DataAccess.Repositories;
using System.Net;

namespace FLY.Business.Services.Implements
{
    public class ShopManagerService : IShopManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopManagerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // shop manager 
        public async Task<ShopResponse> GetShopByIdAsync(int id)
        {
            try
            {
                var shop = await _unitOfWork.ShopRepository.GetByIDAsync(id);
                var result = _mapper.Map<ShopResponse>(shop);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateShopInformation(ShopRequest request)
        {
            var shops = await _unitOfWork.ShopRepository.FindAsync(a => a.ShopId == request.ShopId);
            var existedShops = shops.FirstOrDefault();
            if (existedShops == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your shop is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedShops, request);
                    await _unitOfWork.ShopRepository.UpdateAsync(existedShops);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }


        // Manager product  

        public async Task<List<ProductResponse>> GetAllProductsAsync(int ShopId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.FindAsync(a => a.ShopId == ShopId);
                var result = _mapper.Map<List<ProductResponse>>(product.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(id);
                var result = _mapper.Map<ProductResponse>(product);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateProductInformation(ProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.FindAsync(a => a.ProductId == request.ProductId);
            var existedProducts = product.FirstOrDefault();
            if (existedProducts == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your product is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedProducts, request);
                    await _unitOfWork.ProductRepository.UpdateAsync(existedProducts);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteProductInformation(ProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.FindAsync(a => a.ProductId == request.ProductId);
            var existedProducts = product.FirstOrDefault();
            if (existedProducts == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your product is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedProducts, request);
                    await _unitOfWork.ProductRepository.DeleteAsync(existedProducts);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> CreateProductInformation(ProductResponse response)
        {
            var pd = await _unitOfWork.ProductRepository.FindAsync(a => a.ProductId == response.ProductId);
            var existedPds = pd.FirstOrDefault();

            if (existedPds != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Voucher already exists");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Product newVoucher = _mapper.Map<Product>(response);
                    await _unitOfWork.ProductRepository.InsertAsync(newVoucher);
                    await _unitOfWork.SaveAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        // Manager Order

        //public async Task<List<OrderResponse>> GetAllOrdersAsync(int ShopId)
        //{
        //    try
        //    {
        //        var od = await _unitOfWork.OrderRepository.FindAsync(a => a.ShopId == ShopId);
        //        var result = _mapper.Map<List<OrderResponse>>(od.ToList());
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<bool> UpdateOrderInformation(OrderRequest request)
        {
            var od = await _unitOfWork.OrderRepository.FindAsync(a => a.OrderId == request.OrderId);
            var existedOds = od.FirstOrDefault();
            if (existedOds == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your Order is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedOds, request);
                    await _unitOfWork.OrderRepository.UpdateAsync(existedOds);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }


        public async Task<List<OrderDetailResponse>> GetOrderDetailAsync(int OrderId)
        {
            try
            {
                var odt = await _unitOfWork.OrderDetailRepository.FindAsync(a => a.OrderId == OrderId);
                var result = _mapper.Map<List<OrderDetailResponse>>(odt.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Manager revenue
        //public async Task<float> GetRevenueAsync(int shopId, int month, int year)
        //{
        //    try
        //    {
        //        var orders = await _unitOfWork.OrderRepository.FindAsync(a =>
        //            //a.ShopId == shopId &&
        //            a.OrderDate.Month == month &&
        //            a.OrderDate.Year == year);

        //        var totalRevenue = orders.Sum(o => o.TotalPrice);

        //        return totalRevenue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        // Manager Voucher
        public async Task<List<VoucherOfShopResponse>> GetAllVouchersAsync(int ShopId)
        {
            try
            {
                var od = await _unitOfWork.VoucherOfshopRepository.FindAsync(a => a.ShopId == ShopId);
                var result = _mapper.Map<List<VoucherOfShopResponse>>(od.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<VoucherOfShopResponse> GetVoucherByIdAsync(int VoucherId)
        {
            try
            {
                var odt = await _unitOfWork.VoucherOfshopRepository.GetByIDAsync(VoucherId);
                var result = _mapper.Map<VoucherOfShopResponse>(odt);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateVouchersInformation(VoucherOfShopRequest request)
        {
            var vch = await _unitOfWork.VoucherOfshopRepository.FindAsync(a => a.VoucherId == request.VoucherId);
            var existedVchs = vch.FirstOrDefault();
            if (existedVchs == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your Order is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedVchs, request);
                    await _unitOfWork.VoucherOfshopRepository.UpdateAsync(existedVchs);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<bool> CreateVouchersInformation(VoucherOfShopResponse response)
        {
            var vch = await _unitOfWork.VoucherOfshopRepository.FindAsync(a => a.VoucherId == response.VoucherId);
            var existedVchs = vch.FirstOrDefault();

            if (existedVchs != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Voucher already exists");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    VoucherOfshop newVoucher = _mapper.Map<VoucherOfshop>(response);
                    await _unitOfWork.VoucherOfshopRepository.InsertAsync(newVoucher);
                    await _unitOfWork.SaveAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteVouchersInformation(VoucherOfShopRequest request)
        {
            var vch = await _unitOfWork.VoucherOfshopRepository.FindAsync(a => a.VoucherId == request.VoucherId);
            var existedVchs = vch.FirstOrDefault();
            if (existedVchs == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Your product is not existed");
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _mapper.Map(existedVchs, request);
                    await _unitOfWork.ProductRepository.DeleteAsync(existedVchs);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
