using Microsoft.AspNetCore.Mvc;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HRAdminController : ControllerBase
    {
        private readonly IHRAdminRepository _hrAdminRepository;

        #region Constructor
        public HRAdminController(IHRAdminRepository hrAdminRepository)
        {
            _hrAdminRepository = hrAdminRepository;
        }
        #endregion

        #region Get API for AssignApprover
        [HttpGet("usermanagerinfo")]
        public async Task<IActionResult> GetUserManagerInfo()
        {
            var userManagerInfo = await _hrAdminRepository.GetUserManagerInfoAsync();
            return Ok(userManagerInfo);
        }
        #endregion

        #region POST API for AssignApprover
        [HttpPost("save")]
        public async Task<IActionResult> AddOrUpdateApprovers([FromBody] PostUserManagerDTO usermanager)
        {
            usermanager.ModUserID = UserSession.GetUserId(HttpContext);
            string msg = await _hrAdminRepository.AddOrUpdateApproversAsync(usermanager);
            return Ok(msg);

        }
        #endregion
    }
}
