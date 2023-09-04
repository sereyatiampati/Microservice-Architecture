using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheJitu_commerce_Coupons.Model;
using TheJitu_commerce_Coupons.Model.Dtos;
using TheJitu_commerce_Coupons.Services.IService;

namespace TheJitu_commerce_Coupons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {   

        private readonly IMapper _mapper;
        private readonly ICouponInterface _couponInterface;
        private readonly ResponseDto _responseDto;
        public CouponController(IMapper mapper, ICouponInterface couponInterface)
        {
            _couponInterface = couponInterface;
            _mapper = mapper;
            _responseDto = new ResponseDto();

        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllCoupons()
        {
            var coupons = await _couponInterface.GetCouponsAsync();
            if(coupons == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Occured";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = coupons;
            return Ok(_responseDto);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ResponseDto>> AddCoupon(CouponRequestDto couponRequest)
        {
            var newCoupon = _mapper.Map<Coupon>(couponRequest);
            var response = await _couponInterface.AddCouponAsync(newCoupon);
            if (string.IsNullOrWhiteSpace(response))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Occured";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = response;
            return Ok(_responseDto);
        }

        [HttpGet("GetByName{code}")]
        public async Task<ActionResult<ResponseDto>> GetCoupon(string code )
        {
            var coupon= await _couponInterface.GetCouponByNameAsync(code);
            if (coupon == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Occured";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = coupon;
            return Ok(_responseDto);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> UpdateCoupon(Guid id , CouponRequestDto couponRequestDto)
        {
            var coupon = await _couponInterface.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Occured";
                return BadRequest(_responseDto);
            }
            //update
            var updated =_mapper.Map(couponRequestDto, coupon);
            var response = await _couponInterface.UpdateCouponAsync(updated);
            _responseDto.Result = response;
            return Ok(_responseDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteCoupon(Guid id)
        {
            var coupon = await _couponInterface.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Occured";
                return BadRequest(_responseDto);
            }
            //delete
            
            var response = await _couponInterface.DeleteCouponAsync(coupon);
            _responseDto.Result = response;
            return Ok(_responseDto);
        }

    }
}
