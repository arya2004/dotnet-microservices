﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> CreateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
           {
                ApiType = SD.ApiType.POST,
                Data = product,
                Url = SD.ProductAPIBase + "/api/product/",
                ContentType = SD.ContentType.MultipartFormData
            });

        }

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Data = id,
                Url = SD.ProductAPIBase + "/api/product/" + id.ToString()
            }) ;
        }

        public async Task<ResponseDto> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product"
            });
        }



        public async Task<ResponseDto> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product/" + id.ToString()
            });
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = product,
                Url = SD.ProductAPIBase + "/api/product/",
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
