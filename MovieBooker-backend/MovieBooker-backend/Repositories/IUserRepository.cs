using MovieBooker_backend.DTO;
using MovieBooker_backend;
using MovieBooker_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieBooker_backend.Repositories
{
    public interface IUserRepository
    {
        public Task<TokenResponse> SignInInternalAsync(SignInModel model);
        public Task<int> SignUpInternalAsync(SignUpModel model);
        public Task<TokenResponse> GenerateTokensAsync(User user);

        public IEnumerable<User> GetAllUser();
        public User GetUserById(int userId);
        public User GetUserByEmail(string email);
        public void AddUser(User user);
        public UserDTO GetUserDTOByEmail(string email);


    }
}
