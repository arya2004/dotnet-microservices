using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {   //refer to coupon api controller
        Task<ResponseDto> GetCouponAsync(string couponCode);
        Task<ResponseDto> GetAllCouponAsync();
        Task<ResponseDto> GetCouponByIdAsync(int id);
        Task<ResponseDto> CreateCouponAsync(CouponDto coupon);
        Task<ResponseDto> UpdateCouponAsync(CouponDto coupon);
        Task<ResponseDto> DeleteCouponAsync(int id);

    }
}
