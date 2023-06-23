using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {   //refer to coupon api controller
        Task<ResponseDto> CreateOrderc(CartDto cartDto);
        Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto);

    }
}
