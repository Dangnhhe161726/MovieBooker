namespace MovieBooker.Models
{
    public interface IAuthenService
    {
        Task<string> GetAccessTokenAsync();
    }
}
