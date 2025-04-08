using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userService)
        {
            _userRepository = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

    }
}
