using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {   //refer to coupon api controller
        Task<ResponseDto> CreateOrderc(CartDto cartDto);
        Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto);
        Task<ResponseDto> ValidateStripeSession(int orderHeaderID);
        Task<ResponseDto> GetAllOrder(string? userId);
        Task<ResponseDto> GetOrder(int orderId);

        Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus);

    }
}
