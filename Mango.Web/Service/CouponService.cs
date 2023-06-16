using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseservice;
        public CouponService(IBaseService baseService)
        {
              _baseservice = baseService;
        }

        public async Task<ResponseDto> CreateCouponAsync(CouponDto coupon)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = coupon,
                Url = SD.CouponAPIBase + "/api/coupon/"
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto> GetAllCouponAsync()
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/GetByCode" + couponCode
            }) ;
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto coupon)
        {
            return await _baseservice.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = coupon,
                Url = SD.CouponAPIBase + "/api/coupon/"
            });
        }
    }
}
