using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        public IEnumerable<RoleDTO> GetRole();
    }
}
