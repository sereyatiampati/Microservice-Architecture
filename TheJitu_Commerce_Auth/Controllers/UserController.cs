using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheJitu_Commerce_Auth.Model.Dtos;
using TheJitu_Commerce_Auth.Services.IService;
using TheJituMessageBus;

namespace TheJitu_Commerce_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserInterface _userInterface;
        private readonly ResponseDto _response;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public UserController(IUserInterface userInterface, IConfiguration configuration,IMessageBus message)
        {
            _userInterface = userInterface;
            //Don't inject just initialize
            _response = new ResponseDto();
            _configuration = configuration;
            _messageBus = message;

        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto>> AddUSer(RegisterRequestDto registerRequestDto)
        {
            var errorMessage = await  _userInterface.RegisterUser(registerRequestDto);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                //error
                _response.IsSuccess = false;
                _response.Message = errorMessage;

                return BadRequest(_response);
            }
            //send a message to our ServiceBus --Queue
            var queueName = _configuration.GetSection("QueuesandTopics:RegisterUser").Get<string>();

            var message = new UserMessage()
            {
                Email = registerRequestDto.Email,
                Name = registerRequestDto.Name,
            };
            await _messageBus.PublishMessage(message, queueName);
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> LoginUser(LoginRequestDto loginRequestDto)
        {
            var response = await _userInterface.Login(loginRequestDto);
            if (response.User ==null)
            {
                //error
                _response.IsSuccess = false;
                _response.Message = "Invalid Credential";

                return BadRequest(_response);
            }
            _response.Result= response;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<ActionResult<ResponseDto>> AssignRole(RegisterRequestDto registerRequestDto)
        {
            var response = await _userInterface.AssignUserRole(registerRequestDto.Email, registerRequestDto.Role);
            if (!response)
            {
                //error
                _response.IsSuccess = false;
                _response.Message = "Error Occured";

                return BadRequest(_response);
            }
            _response.Result= response;
            return Ok(_response);
        }
    }
}
