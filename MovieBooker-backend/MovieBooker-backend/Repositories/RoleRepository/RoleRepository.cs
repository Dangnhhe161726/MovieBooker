using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;

namespace MovieBooker_backend.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly bookMovieContext _context;
        public RoleRepository(bookMovieContext context)
        {
            _context = context;
        }

        public IEnumerable<RoleDTO> GetRole()
        {
            var role = _context.Roles.Select(r => new RoleDTO
            {
              RoleId = r.RoleId,
              RoleName = r.RoleName,
            }).ToList();
            return role;
        }
    }
}
