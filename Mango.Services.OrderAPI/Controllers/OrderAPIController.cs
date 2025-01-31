﻿using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Mango.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderAPIController(AppDbContext db, IProductService productService, IMapper mapper, IConfiguration configuration, IMessageBus messageBus)
        {
            _db = db;
            _messageBus = messageBus;
            this._response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public async Task<ResponseDto> GetOrders(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if(!User.IsInRole(SD.RoleAdmin))
                {
                    objList = _db.OrderHeaders.Include(u=> u.OrderDetails).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _db.OrderHeaders.Include(u => u.OrderDetails).Where(u => u.UserId == userId).OrderByDescending(u => u.OrderHeaderId).ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


        [Authorize]
        [HttpGet("GetOrder/{id}")]
        public async Task<ResponseDto> GetOrder(int id)
        {
            try
            {
                OrderHeader order = _db.OrderHeaders.Include(u => u.OrderDetails).First(u => u.OrderHeaderId == id);
                _response.Result = _mapper.Map<OrderHeaderDto>(order);

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {

                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.APprovedUrl,
                    CancelUrl = stripeRequestDto.CanceUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                      
                    Mode = "payment",
                    
                };
                var DiscountsOvj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon = stripeRequestDto.OrderHeader.CouponCode
                    }
                };
                foreach(var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);

                }
                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountsOvj;
                }

                var service = new SessionService();
                Session session = service.Create(options);//it is stripe session
                stripeRequestDto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _db.SaveChanges();
                _response.Result = stripeRequestDto;



            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderIs)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderHeaderIs);


                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);//it is stripe session
                
                var paymentINtentService = new PaymentIntentService();
                PaymentIntent paymentIntent =  paymentINtentService.Get(session.PaymentIntentId);

                if(paymentIntent.Status == "succeeded")
                {
                    //payment successful
                    orderHeader.PaymentIndentId = session.PaymentIntentId;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();

                    RewardsDto rewardsDto = new RewardsDto()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                        UserId = orderHeader.UserId
                    };
                    string topicName = _configuration.GetValue<string>("TopicAndQueueName:OrderCreatedTopic");
                    await _messageBus.PublishMessage(rewardsDto, topicName);
                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }


                


            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody]string newStatus)
        {
            try
            {
                OrderHeader order = _db.OrderHeaders.First(u => u.OrderHeaderId == orderId);
                if (order != null)
                {
                    if(newStatus == SD.Status_Cancelled)
                    {
                        //refund
                        var option = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = order.PaymentIndentId
                        };
                        var service = new RefundService();
                        Refund refund = service.Create(option);
                       
                    }
                    
                        order.Status = newStatus;
                        _db.SaveChanges();
                    
                }
                

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
