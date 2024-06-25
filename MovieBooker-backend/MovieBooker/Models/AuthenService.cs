using StackExchange.Redis;

namespace MovieBooker.Models
{
    public class AuthenService : IAuthenService
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public AuthenService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var redis = _redisConnection.GetDatabase();
            var accessToken = await redis.StringGetAsync("savetoken");
            return accessToken;
        }
    }
}
