using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
   // [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private ResponseDto _resDto;
        private IMapper _mapper;
        public CouponAPIController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _resDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> onjList = _appDbContext.Coupons.ToList();
                _resDto.Result = _mapper.Map<IEnumerable<CouponDto>>(onjList);
                
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }

        [HttpGet]
        [Route("{id}")]
        
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon onjList = _appDbContext.Coupons.First(u=>u.CouponId == id);

                _resDto.Result = _mapper.Map<CouponDto>(onjList); //coupondto is return type
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]

        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon onjList = _appDbContext.Coupons.FirstOrDefault(u => u.CouponCode.ToLower() == code.ToLower());
                if(onjList == null)
                {
                    _resDto.IsSuccess = false;
                }

                _resDto.Result = _mapper.Map<CouponDto>(onjList); //coupondto is return type
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
             

                Coupon Result = _mapper.Map<Coupon>(couponDto);//coupondto is return type
                _appDbContext.Coupons.Add(Result);
                _appDbContext.SaveChanges();
                _resDto.Result = _mapper.Map<CouponDto>(Result);
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {


                Coupon Result = _mapper.Map<Coupon>(couponDto);//coupondto is return type
                _appDbContext.Coupons.Update(Result);
                _appDbContext.SaveChanges();
                _resDto.Result = _mapper.Map<CouponDto>(Result);
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {

                Coupon fromdb = _appDbContext.Coupons.First(u => u.CouponId == id);
                _appDbContext.Coupons.Remove(fromdb);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }
    }
}
