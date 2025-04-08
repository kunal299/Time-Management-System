using Microsoft.AspNetCore.Mvc;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Controllers
{
    [Route("api/timesheet")]
    [ApiController]
    public class TimesheetController : Controller
    {
        #region Dependency Injection
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetController(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        #endregion

        #region Get Timesheet Data
        //Note: we are passing userid just for testing purpose. we'll remove it after integrating with UI
        [HttpGet("{userId}")] 
        public async Task<IActionResult> GetTimesheetData(int userId)
        {
            //int userId = (int)UserSession.GetUserId(HttpContext);
            var timesheet = await _timesheetRepository.GetTimesheetData(userId);

            if (timesheet == null)
                return NotFound($"No timesheet found for UserID: {userId}.");

            return Ok(timesheet);
        }
        #endregion

        #region Add or Update Timesheet Lines
        [HttpPost("TimesheetLine")]
        public async Task<IActionResult> AddOrUpdateTimesheetLine([FromBody] PostTimesheetLineDTO timesheetEntry)
        {
            if (timesheetEntry == null)
            {
                return BadRequest("Timesheet entry is null.");
            }

            const int MaxDayMinutes = 8 * 60;

            int totalMinutes = (timesheetEntry.TotalDayHours * 60) + timesheetEntry.TotalDayMinutes;
            if (totalMinutes > MaxDayMinutes)
            {
                return BadRequest("Total day hours should not exceed 8 hours per day.");
            }

            var lineID = await _timesheetRepository.AddOrUpdateTimesheetLineAsync(timesheetEntry);

            return Ok(new { LineID = lineID });
        }
        #endregion

        #region Delete Timesheet Line
        [HttpDelete("{lineID}")]
        public async Task<IActionResult> DeleteTimesheetLine(int lineID)
        {
            if (lineID <= 0)
            {
                return BadRequest("Invalid LineID.");
            }

            var result = await _timesheetRepository.DeleteTimesheetLineAsync(lineID);
            if (result == "Record deleted successfully.")
            {
                return Ok(new { Message = result });
            }
            else
            {
                return NotFound(new { Message = result });
            }
        }
        #endregion
    }
}