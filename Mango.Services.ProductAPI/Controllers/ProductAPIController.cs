using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Mango.Services.ProductAPI.Controllers
{

    [Route("api/product")]
    [ApiController]
    //[Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private ResponseDto _resDto;
        private IMapper _mapper;
        public ProductAPIController(AppDbContext appDbContext, IMapper mapper)
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
                IEnumerable<Product> onjList = _appDbContext.Products.ToList();
                _resDto.Result = _mapper.Map<IEnumerable<ProductDto>>(onjList);

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
                Product onjList = _appDbContext.Products.First(u => u.ProductId == id);

                _resDto.Result = _mapper.Map<ProductDto>(onjList); //Productdto is return type
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.Message;
            }
            return _resDto;
        }


        [HttpPost]
      //  [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {


                //Product result = _mapper.Map<Product>(productDto);//Productdto is return type
                Product result = new Product();
                result.Name = productDto.Name;
                result.Description = productDto.Description;
                result.Price = productDto.Price;
                result.CategoryName = productDto.CategoryName;
                result.ImageUrl = productDto.ImageUrl;
                _appDbContext.Products.Add(result);
                //_appDbContext.SaveChangesAsync();
                _appDbContext.SaveChanges();
                _resDto.Result = _mapper.Map<ProductDto>(result);
            }
            catch (Exception ex)
            {
                _resDto.IsSuccess = false;
                _resDto.Message = ex.ToString();
            }
            return _resDto;
        }
        [HttpPut]
       // [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {


                Product Result = _mapper.Map<Product>(productDto);//Productdto is return type
                _appDbContext.Products.Update(Result);
                _appDbContext.SaveChanges();
                _resDto.Result = _mapper.Map<ProductDto>(Result);
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
      //  [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {

                Product fromdb = _appDbContext.Products.First(u => u.ProductId == id);
                _appDbContext.Products.Remove(fromdb);
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
