using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLOggedInUser());
        }
       
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            ResponseDto? respnse = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (respnse != null && respnse.IsSuccess)
            {
                TempData["success"] = "cart updated";
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? respnse = await _cartService.ApplyCouponAsync(cartDto);
            if (respnse != null && respnse.IsSuccess)
            {
                TempData["success"] = "cart updated";
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex)); ;
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? respnse = await _cartService.ApplyCouponAsync(cartDto);
            if (respnse != null && respnse.IsSuccess)
            {
                TempData["success"] = "cart updated";
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex));
        }

        private async Task<CartDto> LoadCartDtoBasedOnLOggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value;
            ResponseDto? respnse = await _cartService.GetCartByUserIdAsync(userId);
            if(respnse != null && respnse.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(respnse.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
