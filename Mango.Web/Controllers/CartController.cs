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
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;

        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLOggedInUser());
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLOggedInUser());
        }

        //[Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLOggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var respinse = await _orderService.CreateOrderc(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(respinse.Result));
            if(respinse !=null && respinse.IsSuccess)
            {
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                //get stripe session and redirect  to stripe
                StripeRequestDto stripeRequestDto = new()
                { 
                    APprovedUrl = domain+ "cart/Conformatiion?orderId"+ orderHeaderDto.OrderHeaderId,
                    CanceUrl = domain+ "cart/Checkout",
                    OrderHeader = orderHeaderDto,   
                };
                var stripeRsponse = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeRsponse.Result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                
                return new StatusCodeResult(303);

            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Conformation(int orderId)
        {
            return View();
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
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLOggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault().Value;

            ResponseDto? respnse = await _cartService.EmailCart(cart);
            if (respnse != null && respnse.IsSuccess)
            {
                TempData["success"] = "email will be sent shortly";
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
