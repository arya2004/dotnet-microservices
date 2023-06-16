using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {   
        private readonly ICouponService _couponService;
        public CouponController(ICouponService coupon)
        {
            _couponService = coupon;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto? response = await _couponService.GetAllCouponAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto coupon)
        {
             if(ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(coupon);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "create success";
                    return RedirectToAction("CouponIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }  
            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int id)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(id);
            if (response != null && response.IsSuccess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound(); 
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto  couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Deleted success";
                return RedirectToAction("CouponIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);
        }
    }
}
