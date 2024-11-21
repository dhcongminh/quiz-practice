using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using QuizPracticeApi.Services;

namespace QuizPracticeApi.Controllers {
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly UserServices _userServices;
        public UsersController(UserServices userServices) {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto) {

            return Ok(_userServices.Register(userDto));
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll() {
            return Ok(_userServices.GetAll());
        }

        [HttpGet("{username}")]
        [Authorize]
        public IActionResult GetByUsername(string username) {
            return Ok(_userServices.GetByUsername(username));
        }
    }
}
