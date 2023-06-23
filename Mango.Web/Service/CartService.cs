using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{   
   
    public class CartService : ICartService
    {
        private readonly IBaseService _baseservice;
        public CartService(IBaseService baseService)
        {
            _baseservice = baseService;
        }
        public async Task<ResponseDto> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto> EmailCart(CartDto cartDto)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/EmailCartRequest"
            });
        }

        public async Task<ResponseDto> GetCartByUserIdAsync(string userId)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int CartDetailsId)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = CartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDto> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/CartUpsert"
            });
        }
    }
}
