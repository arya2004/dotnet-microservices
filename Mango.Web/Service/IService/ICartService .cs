using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {   //refer to coupon api controller
        Task<ResponseDto> GetCartByUserIdAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int CartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto> EmailCart(CartDto cartDto);


    }
}
