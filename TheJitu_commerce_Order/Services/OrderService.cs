using AutoMapper;
using TheJitu_commerce_Order.Data;
using TheJitu_commerce_Order.Model.Dto;
using TheJitu_commerce_Order.Models;
using TheJitu_commerce_Order.Models.Dto;
using TheJitu_commerce_Order.Services.Iservice;

namespace TheJitu_commerce_Order.Services
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, AppDbContext  appDbContext)
        {
            _context = appDbContext;
            _mapper = mapper;
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
    }
}
