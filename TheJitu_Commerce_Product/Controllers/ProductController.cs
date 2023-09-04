using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TheJitu_Commerce_Product.Models;
using TheJitu_Commerce_Product.Models.Dtos;
using TheJitu_Commerce_Product.Services.IService;

namespace TheJitu_Commerce_Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductInterface _productInterface;
        private readonly ResponseDto _responseDto;
        public ProductController(IMapper mapper, IProductInterface productInterface)
        {
            _mapper = mapper;
            _productInterface = productInterface;
            _responseDto = new ResponseDto();
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllProducts()
        {
            
                var products = await _productInterface.GetProductsAsync();
                if (products == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Error Occured";
                    return BadRequest(_responseDto);
                }

                _responseDto.Result = products;
               
            
            return Ok(_responseDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> AddCoupon(ProductRequestDto productRequestDto)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(productRequestDto);
                var response = await _productInterface.AddProductAsync(newProduct);
                if (string.IsNullOrWhiteSpace(response))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Error Occured";
                    return BadRequest(_responseDto);
                }

                _responseDto.Result = response;
               
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

        [HttpGet("GetById({Id})")]
        public async Task<ActionResult<ResponseDto>> GetCoupon(Guid Id)
        {
           try{
                var product = await _productInterface.GetProductByIdAsync(Id);
                if (product == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Error Occured";
                    return BadRequest(_responseDto);
                }

                _responseDto.Result = product;
               
            }catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> UpdateCoupon(Guid id, ProductRequestDto productRequestDto)
        {
            try
            {
                var product = await _productInterface.GetProductByIdAsync(id);
                if (product == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Error Occured";
                    return BadRequest(_responseDto);
                }
                //update
                var updated = _mapper.Map(productRequestDto, product);
                var response = await _productInterface.UpdateProductAsync(updated);
                _responseDto.Result = response;
               
            }catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return BadRequest(_responseDto);

            }
            return Ok(_responseDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _productInterface.GetProductByIdAsync(id);
                if (product == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Error Occured";
                    return BadRequest(_responseDto);
                }
                //delete
                var response = await _productInterface.DeleteProductAsync(product);
                _responseDto.Result = response;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                return BadRequest(_responseDto);

            }
            return Ok(_responseDto);
        }
    }
}
