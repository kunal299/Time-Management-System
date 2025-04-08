using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface ITimesheetRepository
    {
        Task<GetTimesheetHdrDTO> GetTimesheetData(int userId);
        Task<int> AddOrUpdateTimesheetLineAsync(PostTimesheetLineDTO timesheetEntry);
        Task<string> DeleteTimesheetLineAsync(int lineID);
    }
}