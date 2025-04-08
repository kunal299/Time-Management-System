namespace UCITMS.Models
{
    #region Get timesheet header
    public class GetTimesheetHdrDTO
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int HoursTotal { get; set; }
        public int MinutesTotal { get; set; }
        public List<GetTimesheetLineDTO>? TimesheetLines { get; set; }
    }
    #endregion

    #region Post timesheet header
    public class PostTimesheetHdrDTO
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int HoursTotal { get; set; }
        public int MinutesTotal { get; set; }
    }
    #endregion

    #region Get timehseet lines
    public class GetTimesheetLineDTO
    {
        public int LineID { get; set; }
        public int TimesheetID { get; set; }
        public int EngagementID { get; set; }
        public int TaskID { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
    }
    #endregion

    #region Post timesheet lines
    public class PostTimesheetLineDTO
    {
        public int? LineID { get; set; }
        public int TimesheetID { get; set; }
        public int EngagementID { get; set; }
        public int TaskID { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public int TotalDayHours { get; set; }
        public int TotalDayMinutes { get; set; }
        public int? ModUser { get; set; }
    }
    #endregion
}