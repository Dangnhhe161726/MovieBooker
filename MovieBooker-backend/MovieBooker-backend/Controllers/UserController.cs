using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using MovieBooker_backend.Repositories.UserRepository;
using StackExchange.Redis;
using System.Security.Claims;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConnectionMultiplexer _redis;
        public UserController(IUserRepository userRepository, IConnectionMultiplexer redis)
        {
            _userRepository = userRepository;
            _redis = redis;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _userRepository.SignUpInternalAsync(model);
            if (result > 0)
            {
                return Ok();
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var tokens = await _userRepository.SignInInternalAsync(model);
            if (tokens == null)
            {
                return Unauthorized();
            }

            return Ok(tokens);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var db = _redis.GetDatabase();
            var userIdValue = await db.StringGetAsync($"RefreshToken:{model.RefreshToken}");
            if (userIdValue.IsNullOrEmpty)
            {
                return Unauthorized();
            }

            // Chuyển đổi userIdValue từ RedisValue sang string
            string userIdString = userIdValue.ToString();
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            // Thực hiện truy vấn với userId
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var tokens = await _userRepository.GenerateTokensAsync(user);

            return Ok(tokens);
        }

        [HttpGet("CheckSignUpEmail/{email}")]
        public IActionResult CheckSignUpEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user != null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("logingoogle")]
        public IActionResult LoginGoogle(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                user = new User { Email = email, UserName = "Member", RoleId = 3 }; 
                _userRepository.AddUser(user);
            }         
            var tokens = _userRepository.GenerateTokensAsync(user);
            return Ok(tokens);
        }


        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Customer")]
        public IActionResult getAll()
        {
            var user = _userRepository.GetAllUser();
            return Ok(user);
        }
    }
}
