using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngagementController : ControllerBase
    {
        #region Data Repositories
        
        private readonly IEngagementRepository _engagementRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor Injection
        public EngagementController(IEngagementRepository engagementRepository, ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _engagementRepository = engagementRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region Engagement Management

        #region Save an engagement
        [HttpPost("Save")]
        public async Task<IActionResult> SaveEngagement([FromBody] PostEngagementDTO engagement)
        {
            engagement.ModUser = UserSession.GetUserId(HttpContext);
            int engagementId = await _engagementRepository.SaveEngagementAsync(engagement);
            return Ok(new { EngagementID = engagementId });
        }
        #endregion

        #region Get all engagements
        [HttpGet]
        public async Task<ActionResult<List<GetEngagementDTO>>> GetAllEngagements()
        {
            var engagements = await _engagementRepository.GetAllEngagements();
            if (engagements == null)
            {
                return NotFound("No engagements found.");
            }
            return Ok(engagements);
        }
        #endregion

        #region Get engagement by engagement id
        [HttpGet("{id}")]
        public ActionResult<GetEngagementDTO> GetEngagementById(int id)
        {
            var engagement = _engagementRepository.GetEngagementById(id);
            if (engagement == null)
            {
                return NotFound($"Engagement with ID {id} not found.");
            }
            return Ok(engagement);
        }
        #endregion

        #region Get engagement by user id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEngagementsByUserId(int? userId)
        {
            if (userId == null || userId <= 0)
            {
                return BadRequest("A valid User ID must be provided.");
            }

            var engagements = await _engagementRepository.GetEngagementsByUserIdAsync(userId);

            if (engagements == null || engagements.Count == 0)
            {
                return NotFound("No engagements found for the specified user.");
            }

            return Ok(engagements);
        }
        #endregion

        #region Get engagement by owner
        [HttpGet("owner")]
        public async Task<IActionResult> GetEngagementByOwner()
        {
            var userId = UserSession.GetUserId(HttpContext);
            var engagements = (await _engagementRepository.GetAllEngagements()).Where(e => e.Owners.Any(o => o.UserID == userId)).ToList();
            return Ok(engagements);
        }
        #endregion

        #region Get engagement by user and date
        [HttpGet("GetEngagementsByUserAndDate/{userId}/{date}")]
        public IActionResult GetEngagementsByUserAndDate(int userId, [FromRoute] DateTime date)
        {
            List<GetEngagementDTO> engagements = _engagementRepository.GetEngagementsByUserAndDate(userId, date);

            if (engagements == null || engagements.Count == 0)
            {
                return NotFound("No engagements found for the given user and date.");
            }

            return Ok(engagements);
        }
        #endregion

        #region Get Engagements for Employee
        [HttpGet("engagements/{id}")]
        public async Task<ActionResult<IEnumerable<GetAllEngagementsDTO>>> GetEngagementsforEmployee(int? id)
        {
            if (id == null)
            {
                return BadRequest("User Id not found in session.");
            }
            try
            {
                var engagements = await _engagementRepository.GetEngagementsforEmployeeAsync(id);

                if (engagements == null || !engagements.Any())
                {
                    return NotFound("No engagements found for the specified User.");
                }

                return Ok(engagements);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving engagements: {ex.Message}");
            }
        }
        #endregion

        #endregion

        #region Task Management

        #region Get all tasks
        [HttpGet("tasks")]
        public async Task<ActionResult<IEnumerable<GetTaskDTO>>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found.");
            }
            return Ok(tasks);
        }
        #endregion

        #region Get task by task id
        [HttpGet("tasks/{id}")]
        public async Task<ActionResult<GetTaskDTO>> GetTaskById(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            return Ok(task);
        }
        #endregion

        #region Add a task
        [HttpPost("tasks")]
        public async Task<IActionResult> AddTask([FromBody]PostTaskDTO task)
        {
            int userId = (int)UserSession.GetUserId(HttpContext);
            task.ModUser = userId;
            int taskId = await _taskRepository.AddTask(task);
            return Ok(new { TaskID = taskId });
        }
        #endregion

        #endregion

        #region Owner Management
        
        #region Get all owners
        [HttpGet("owners")]
        public async Task<IActionResult> GetOwners()
        {
            var owners = await _engagementRepository.GetProjectOwnersAsync();
            if (owners == null || !owners.Any())
            {
                return NotFound("No project owners found.");
            }
            return Ok(owners);
        }
        #endregion

        #endregion

        #region Team Member Management

        #region Get team members using manager id
        [HttpGet("GetTeamMembers/{managerId}")]
        public async Task<ActionResult<List<UserDTO>>> GetTeamMembers(int managerId)
        {
            var users = await _userRepository.GetUsersUnderManager(managerId);
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found under the specified manager.");
            }
            return Ok(users);
        }
        #endregion

        #endregion
    }
}