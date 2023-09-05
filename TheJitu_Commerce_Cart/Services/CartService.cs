using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheJitu_Commerce_Cart.Data;
using TheJitu_Commerce_Cart.Model;
using TheJitu_Commerce_Cart.Model.Dto;
using TheJitu_Commerce_Cart.Services.Iservice;

namespace TheJitu_Commerce_Cart.Services
{
    public class CartService : ICartservice
    {   

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IProductInterface _productInterface;
        private readonly ICouponService _couponService;
        public CartService(AppDbContext db, IMapper mapper, IProductInterface productInterface, ICouponService couponService)
        {
            _appDbContext = db;
            _mapper = mapper;
            _productInterface = productInterface;
            _couponService = couponService;

        }
        public async  Task<bool> ApplyCoupons(CartDto cartDto)
        {
            //get the Header 
            CartHeader CartHeaderFromDb = await _appDbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId ==
             cartDto.CartHeader.UserId
             );

            CartHeaderFromDb.CouponCode= cartDto.CartHeader.CouponCode;
            _appDbContext.CartHeaders.Update(CartHeaderFromDb);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CartUpsert(CartDto cartDto)
        {
            //is this the first item the user has added to the Database?
            CartHeader CartHeaderFromDb = await _appDbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId ==
            cartDto.CartHeader.UserId
            );
            if (CartHeaderFromDb == null)
            {
                // Create a Cart Header and CartDetails
                var newCartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                 _appDbContext.CartHeaders.Add(newCartHeader);
                await _appDbContext.SaveChangesAsync();

                //Use the Id above for CartDetails
                //assign CartHeaderId
                cartDto.CartDetails.First().CartHeaderId = newCartHeader.CartHeaderId;
                var cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                _appDbContext.CartDetails.Add(cartDetails);
                await _appDbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                // I'm either Adding a new Item or updating the count of an Existing Item
                CartDetails CartDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(x=>x.ProductId==
                cartDto.CartDetails.First().ProductId && x.CartHeaderId==CartHeaderFromDb.CartHeaderId);

                if (CartDetailsFromDb == null)
                {
                    //its a different Product
                    cartDto.CartDetails.First().CartHeaderId = CartHeaderFromDb.CartHeaderId;
                    var cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    _appDbContext.CartDetails.Add(cartDetails);
                    await _appDbContext.SaveChangesAsync();

                }
                else
                {
                    CartDetailsFromDb.Count += cartDto.CartDetails.First().Count;
                    _appDbContext.CartDetails.Update(CartDetailsFromDb);
                    await _appDbContext.SaveChangesAsync();
                    //updating Count 
                }

                return true;
            }
            return false;
        }

        public async Task<CartDto> GetUserCart(Guid userId)
        {
           var cartHeader= await _appDbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            var cartDetails= _appDbContext.CartDetails.Where(x => x.CartHeaderId == cartHeader.CartHeaderId);
            CartDto cart= new CartDto()
            {
               CartHeader= _mapper.Map<CartHeaderDto>(cartHeader),
               CartDetails=_mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails)
            };
            //Calculate Cart Total
            var products =await _productInterface.GetProductaAsync();

            foreach (var item in cart.CartDetails)
            {
                item.Product= products.FirstOrDefault(x=>x.ProductId==item.ProductId);
                cart.CartHeader.CartTotal += (int) (item.Count * item.Product.Price);
            }
            //Apply Coupon
            if (!string.IsNullOrWhiteSpace(cart.CartHeader.CouponCode))
            {
                //there is a coupon
                var coupon = await _couponService.GetCouponData(cart.CartHeader.CouponCode);
                if(coupon != null && cart.CartHeader.CartTotal> coupon.CouponMinAmont) 
                {
                    cart.CartHeader.CartTotal -= coupon.CouponAmount;
                    cart.CartHeader.Discount=coupon.CouponAmount;
                }
            }
            return cart;
            
        }

        public async Task<bool> RemoveFromCart(Guid CartDetailId)
        {

            CartDetails cartDetails = await _appDbContext.CartDetails.FirstOrDefaultAsync(x => x.CartDetailsId == CartDetailId);

            // is this the last item the user is deleting 
            var itemsCount = _appDbContext.CartDetails.Where(c=>c.CartHeaderId==cartDetails.CartHeaderId).Count();
            
            _appDbContext.CartDetails.Remove(cartDetails);

            if(itemsCount ==1) 
            {
                _appDbContext.CartHeaders.Remove(_appDbContext.CartHeaders.FirstOrDefault(x => x.CartHeaderId == cartDetails.CartHeaderId));
            }

            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
