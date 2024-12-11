using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using QuizPracticeApi.Helpers;
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

        [HttpGet("login")]
        public IActionResult Login(string username, string password) {
            return Ok(_userServices.Login(username, password));
        }

        [HttpPut("activate")]
        public IActionResult Activate(string token, string username) {
            return Ok(_userServices.Activate(token, username));
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetByUsername(string username) {
            var usernameClaim = User?.FindFirst("username")?.Value;
            if (usernameClaim != username) {
                return Forbid();
            }
            return Ok(_userServices.GetByUsername(username));
        }
    }
}
