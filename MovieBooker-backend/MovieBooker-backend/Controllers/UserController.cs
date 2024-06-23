using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
                return NotFound();
            }
            if(tokens.AccessToken == "1" && tokens.RefreshToken == "1")
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

        [HttpPost("GenerateTokens")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateTokens(User user)
        {
            var tokens = await _userRepository.GenerateTokensAsync(user);
            if (tokens == null)
            {
                return Unauthorized();
            }
            return Ok(tokens);
        }

        [HttpGet("CheckSignUpEmail/{email}")]
        public IActionResult CheckSignUpEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user != null)
            {
                if (user.Status == false)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(user);
                }
            }
            else
            {
                return NotFound();
            }
        }
        [AllowAnonymous]
        [HttpPost("LoginGoogle")]
        public async Task<IActionResult> LoginGoogle( User user)
        {
            var tokens = await _userRepository.LoginGoogle(user);
            if (tokens == null)
            {
                return Unauthorized();
            }
            return Ok(tokens);
        }

        [EnableQuery]
        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            var user = _userRepository.GetAllUser();
            return Ok(user);
        }

        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, UserDTO updatedUser)
        {
            _userRepository.UpdateUser(id, updatedUser);
            return Ok();
        }

        [HttpPut("ChangeStatusUser/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeStatusUser(int id)
        {
            _userRepository.ChangeStatusUser(id);
            return Ok();
        }
    }
}
