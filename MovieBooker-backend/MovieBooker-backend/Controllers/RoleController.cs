using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.RoleRepository;
using MovieBooker_backend.Repositories.ScheduleRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository  _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("GetRole")]
        public ActionResult Get()
        {
           var role = _roleRepository.GetRole();
            if (role != null)
            {
                return Ok(role);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
