using AutoMapper;
using FLY.Business.Models.Account;
using FLY.Business.Models.Blog;
using FLY.Business.Models.Carte;
using FLY.Business.Models.Customer;
using FLY.Business.Models.Feedback;
using FLY.Business.Models.Order;
using FLY.Business.Models.OrderDetail;
using FLY.Business.Models.Product;
using FLY.Business.Models.ProductCategory;
using FLY.Business.Models.Rating;
using FLY.Business.Models.Shop;
using FLY.Business.Models.VoucherOfShop;
using FLY.DataAccess.Entities;

namespace FLY.Business.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            ///Mapper Authentication
            CreateMap<Account, AuthResponse>();
            CreateMap<RegisterRequest, Account>();
            ///Mapper Shop
            CreateMap<Shop, ShopResponse>();
            CreateMap<ShopRequest, Shop>();
            ///Mapper Product
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();
            ///Mapper Feedback
            CreateMap<Feedback, FeedbackResponse>()
                .ForMember(dest => dest.ShopName, src => src.MapFrom(x => x.Shop.ShopName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Account.Email));
            CreateMap<FeedbackRequest, Feedback>();
            CreateMap<UpdateFeedbackRequest, Feedback>();
            ///Mapper Rating
            CreateMap<Rating, RatingResponse>()
                .ForMember(dest => dest.ShopName, src => src.MapFrom(x => x.Shop.ShopName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Account.Email));
            CreateMap<RatingRequest, Rating>();
            ///Mapper ProductCategory
            CreateMap<ProductCategory, ProductCategoryResponse>();
            ///Mapper Customer
            CreateMap<UpdateInfoRequest, Account>();
            ///Mapper Voucher
            CreateMap<VoucherOfshop, VoucherOfShopResponse>();
            CreateMap<VoucherOfShopRequest, VoucherOfshop>();
            ///Mapper Order
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderResponse, Order>();
            CreateMap<CreateOrderRequest, Order>();
            ///Mapper OrderDetail
            CreateMap<OrderDetail, OrderDetailResponse>();
            CreateMap<OrderDetailRequest, OrderDetail>();
            ///Mapper Carts
            CreateMap<CartRequest, Cart>();
            CreateMap<SubCartRequest, Cart>();
            CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email));
            CreateMap<Cart, ProductInCartResponse>()
                .ForMember(dest => dest.CartQuantity, src => src.MapFrom(x => x.CartQuantity))
                .ForMember(dest => dest.ProductPrice, src => src.MapFrom(x => x.Product.ProductPrice))
                .ForMember(dest => dest.ProductName, src => src.MapFrom(x => x.Product.ProductName))
                .ForMember(dest => dest.ProductCategoryName, src => src.MapFrom(x => x.Product.ProductCategory.ProductCategoryName));
            //Mapper Blog
            CreateMap<Blog, BlogResponse>();
            CreateMap<BlogRequest, Blog>();
        }
    }
}
