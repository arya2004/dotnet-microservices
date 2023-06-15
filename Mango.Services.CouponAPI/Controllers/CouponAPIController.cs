using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
