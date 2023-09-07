using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using TheJitu_commerce_Order.Data;
using TheJitu_commerce_Order.Model.Dto;
using TheJitu_commerce_Order.Models;
using TheJitu_commerce_Order.Models.Dto;
using TheJitu_commerce_Order.Services.Iservice;
using TheJituMessageBus;

namespace TheJitu_commerce_Order.Services
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        public OrderService(IMapper mapper, AppDbContext  appDbContext, IMessageBus messsageBus)
        {
            _context = appDbContext;
            _mapper = mapper;
            _messageBus = messsageBus;
        }
        public async Task<OrderHeaderDto> CreateOrderHeader(CartDto cartDto)
        {
             OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.Status = "Pending";
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);
            orderHeaderDto.OrderTotal= Math.Round(orderHeaderDto.OrderTotal,2);
            OrderHeader newOrder = _mapper.Map<OrderHeader>(orderHeaderDto);
            //await Console.Out.WriteLineAsync(newOrder.UserId);
            var item =_context.OrderHeaders.Add(newOrder).Entity;
            await _context.SaveChangesAsync();


            orderHeaderDto.OrderHeaderId= item.OrderHeaderId;
            return orderHeaderDto;
        }

        public async Task<StripeRequestDto> StripePayment(StripeRequestDto stripeRequestDto)
        {
            var options = new SessionCreateOptions()
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };


           foreach( var item in stripeRequestDto.OrderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "kes",

                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = item.ProductName
                        },


                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            var DiscountObj = new List<SessionDiscountOptions>()
           {
               new SessionDiscountOptions()
               {
                   Coupon= stripeRequestDto.OrderHeader.CouponCode
               }
           };

            if (stripeRequestDto.OrderHeader.Discount > 0)
            {
                options.Discounts = DiscountObj;
            }

            var service = new SessionService();
            Session session= service.Create(options);

            //URL
            //Session ID- portion of the URL
            stripeRequestDto.StripeSessionId = session.Id;
            stripeRequestDto.StripeSessionUrl = session.Url;

            OrderHeader order = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);

            order.StripeSessionId = session.Id;
            await _context.SaveChangesAsync();

            return stripeRequestDto;

        }

        public async  Task<bool> ValidatePayment(Guid OrderId)
        {
            OrderHeader order = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.OrderHeaderId == OrderId);

            var service = new SessionService();
            Session session = service.Get(order.StripeSessionId);

            var paymentIntentService = new PaymentIntentService();
            var id = session.PaymentIntentId;
            if(id==null)
            {
                return false;
            }
            PaymentIntent paymentInt = paymentIntentService.Get(id);

            if (paymentInt.Status == "succeeded")
            {
                order.PaymentIntentId = paymentInt.Id;
                order.Status = "Approved";
                await  _context.SaveChangesAsync();
                var rewards = new RewardsDto()
                {
                    Email = "joepay592@gmail.com",
                    TotalAmount = (int)order.OrderTotal,
                    UserId = order.UserId

                };
                //Communicate with Rewards Topic
                await _messageBus.PublishMessage(rewards, "ordertopic");
                return true;
                
            }
            return false;

        }
    }
}
