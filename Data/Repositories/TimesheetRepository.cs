using Microsoft.Data.SqlClient;
using System.Data;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Data.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        #region Dependency Injection
        private readonly string _connectionString;

        public TimesheetRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Get Timesheet Data
        public async Task<GetTimesheetHdrDTO> GetTimesheetData(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetTimesheet_v2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        GetTimesheetHdrDTO timesheet = null;

                        #region Read the timesheet header details
                        if (await reader.ReadAsync())
                        {
                            timesheet = new GetTimesheetHdrDTO
                            {
                                TimesheetID = reader.GetInt32(reader.GetOrdinal("TimesheetID")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                Status = reader.GetInt32(reader.GetOrdinal("Status")),
                                HoursTotal = reader.GetInt32(reader.GetOrdinal("TotalHours")),
                                MinutesTotal = reader.GetInt32(reader.GetOrdinal("TotalMinutes")),
                                TimesheetLines = new List<GetTimesheetLineDTO>()
                            };
                        }

                        // Move to the next result set for timesheet lines, if any
                        if (timesheet != null && await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var line = new GetTimesheetLineDTO
                                {
                                    LineID = reader.GetInt32(reader.GetOrdinal("LineID")),
                                    TimesheetID = reader.GetInt32(reader.GetOrdinal("TimesheetID")),
                                    EngagementID = reader.GetInt32(reader.GetOrdinal("EngagementID")),
                                    TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                                    Hours = reader.GetInt32(reader.GetOrdinal("Hours")),
                                    Minutes = reader.GetInt32(reader.GetOrdinal("Minutes")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment"))
                                };

                                timesheet.TimesheetLines.Add(line);
                            }
                        }
                        #endregion
                        return timesheet;
                    }
                }
            }
        }
        #endregion

        #region Add or Update Timesheet Line
        public async Task<int> AddOrUpdateTimesheetLineAsync(PostTimesheetLineDTO timesheetEntry)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("AddOrUpdateTimesheetLine_v1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    #region Add parameters
                    command.Parameters.AddWithValue("@LineID", timesheetEntry.LineID.HasValue ? (object)timesheetEntry.LineID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@TimesheetID", timesheetEntry.TimesheetID);
                    command.Parameters.AddWithValue("@EngagementID", timesheetEntry.EngagementID);
                    command.Parameters.AddWithValue("@TaskID", timesheetEntry.TaskID);
                    command.Parameters.AddWithValue("@Hours", timesheetEntry.Hours);
                    command.Parameters.AddWithValue("@Minutes", timesheetEntry.Minutes);
                    command.Parameters.AddWithValue("@Date", timesheetEntry.Date);
                    command.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(timesheetEntry.Comment) ? (object)DBNull.Value : timesheetEntry.Comment);
                    command.Parameters.AddWithValue("@ModUser", timesheetEntry.ModUser.HasValue ? (object)timesheetEntry.ModUser.Value : DBNull.Value);
                    #endregion

                    await connection.OpenAsync();

                    // Execute the command and retrieve the new or updated LineID
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
        #endregion

        #region Delete Timesheet Line
        public async Task<string> DeleteTimesheetLineAsync(int lineID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteTimesheetLine", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LineID", lineID);

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return reader["Message"].ToString();
                        }
                        else
                        {
                            return "No response from database.";
                        }
                    }
                }
            }
        }
        #endregion
    }
}