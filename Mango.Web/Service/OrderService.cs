using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Identity;

namespace Mango.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseservice;
        public OrderService(IBaseService baseService)
        {
              _baseservice = baseService;
        }

        

        public async Task<ResponseDto> CreateOrderc(CartDto cartDto)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrderAPIBase + "/api/order/CreateOrder"
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderAPIBase + "/api/order/CreateStripeSession"
            });
        }

        public async Task<ResponseDto> GetAllOrder(string? userId)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + "/api/order/GetOrders/" + userId
            });
        }

        public async Task<ResponseDto> GetOrder(int orderId)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + "/api/order/GetOrder/" + orderId
            });
        }

        public async Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrderAPIBase + "/api/order/UpdateOrderStatus/" + orderId
            }) ;
        }

        public  async Task<ResponseDto> ValidateStripeSession(int orderHeaderID)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderID,
                Url = SD.OrderAPIBase + "/api/order/ValidateStripeSession"
            });
        }
    }
}
