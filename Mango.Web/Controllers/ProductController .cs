using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {   
        private readonly IProductService _ProductService;
        public ProductController(IProductService Product)
        {
            _ProductService = Product;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            ResponseDto? response = await _ProductService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto Product)
        {
             if(ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.CreateProductAsync(Product);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "create success";
                    return RedirectToAction("ProductIndex", "Product");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }  
            return View(Product);
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            ResponseDto? response = await _ProductService.GetProductByIdAsync(id);
            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound(); 
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto  ProductDto)
        {
            ResponseDto? response = await _ProductService.DeleteProductAsync(ProductDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Deleted success";
                return RedirectToAction("ProductIndex", "Product");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(ProductDto);
        }
    }
}
