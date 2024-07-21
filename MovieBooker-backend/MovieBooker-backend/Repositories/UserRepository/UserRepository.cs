using MovieBooker_backend.DTO;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Models;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using JWT.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBooker_backend.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly bookMovieContext _context;
        private readonly IConnectionMultiplexer _redis;

        public UserRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            bookMovieContext context, IConnectionMultiplexer redis)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            _context = context;
            _redis = redis;
        }


        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public async Task<TokenResponse> GenerateTokensAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var db1 = _redis.GetDatabase();
            await db1.StringSetAsync("savetoken", accessToken, TimeSpan.FromMinutes(2));

            var refreshToken = Guid.NewGuid().ToString();
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"RefreshToken:{refreshToken}", user.UserId, TimeSpan.FromDays(7));

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponse> LoginGoogle(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            var users = GetUserByEmail(user.Email);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, users.Email),
                new Claim(ClaimTypes.Role, users.Role.RoleName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var db1 = _redis.GetDatabase();
            await db1.StringSetAsync("savetoken", accessToken, TimeSpan.FromMinutes(2));

            var refreshToken = Guid.NewGuid().ToString();
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"RefreshToken:{refreshToken}", users.UserId, TimeSpan.FromDays(7));

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        public IEnumerable<UserDTO> GetAllUser()
        {
            var listUser = _context.Users.Include(u => u.Role).Select(u => new UserDTO
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Gender = u.Gender,
                Dob = u.Dob,
                Role = u.Role,
                Status = u.Status,
                Password = u.Password,
                Avatar = u.Avatar,
                RoleId = u.RoleId,
            }).ToList();
            return listUser;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users
               .Include(u => u.Role)
               .Where(u => u.Email == email)
               .Select(u => new User
               {
                   UserId = u.UserId,
                   UserName = u.UserName,
                   Email = u.Email,
                   PhoneNumber = u.PhoneNumber,
                   Address = u.Address,
                   Gender = u.Gender,
                   Dob = u.Dob,
                   Role = u.Role, 
                   Status = u.Status,
                   Password = u.Password, 
                   Avatar = u.Avatar
               })
               .FirstOrDefault();

            return user;
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users
          .Include(u => u.Role)
          .Where(u => u.UserId == userId)
          .Select(u => new User
          {
              UserId = u.UserId,
              UserName = u.UserName,
              Email = u.Email,
              PhoneNumber = u.PhoneNumber,
              Address = u.Address,
              Gender = u.Gender,
              Dob = u.Dob,
              Role = u.Role,
              Status = u.Status,
              Password = u.Password,
              Avatar = u.Avatar
          })
          .FirstOrDefault();
            return user;
        }


        public async Task<TokenResponse> SignInInternalAsync(SignInModel model)
        {
            var user = await _context.Users.Include(u => u.Role)
              .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null)
            {
                return null;
            }
            if(user.Status == false)
            {
                return new TokenResponse
                {
                    AccessToken = "1",
                    RefreshToken = "1"
                };
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var db1 = _redis.GetDatabase();
            await db1.StringSetAsync("savetoken", accessToken, TimeSpan.FromMinutes(2));

            var refreshToken = Guid.NewGuid().ToString();
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"RefreshToken:{refreshToken}", user.UserId, TimeSpan.FromDays(7));

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<int> SignUpInternalAsync(SignUpModel model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return -1;
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                Dob = model.Dob,
                RoleId = model.Role,
                Status = model.Status
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public void UpdateUser(string email, UpdateUserDTO user)
        {
           var existingUser = _context.Users.FirstOrDefault(u=>u.Email == email);
          if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Address = user.Address;
                existingUser.Gender = user.Gender;
                existingUser.Avatar = user.Avatar;
                existingUser.Dob = user.Dob;
                existingUser.RoleId = user.RoleId;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
            }
        }

        public void ChangeStatusUser(int id)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existingUser != null)
            {
                if(existingUser.Status == true)
                {
                    existingUser.Status = false;
                }
                else
                {
                    existingUser.Status = true;
                }
                _context.Users.Update(existingUser);
                _context.SaveChanges();
            }
        }

        public void ResetPasswordUser(string email, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email); 
            if (existingUser != null)
            {
                existingUser.Password = password;   
                _context.Users.Update(existingUser); 
                _context.SaveChanges();
            }
        }
    }
}
