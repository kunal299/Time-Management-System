using System.Data;
using UCITMS.Data.IRepositories;
using UCITMS.Models;
using Microsoft.Data.SqlClient;

namespace UCITMS.Data.Repositories
{
    public class HRAdminRepository : IHRAdminRepository
    {
        private readonly string _connectionString;

        #region Constructor
        public HRAdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion
        
        #region Save Approver Details
        public async Task<string> AddOrUpdateApproversAsync(PostUserManagerDTO usermanager)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.AddOrUpdateApprover_v1", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                #region Adding Parameters for SP
                cmd.Parameters.AddWithValue("@UserID", usermanager.UserID);
                cmd.Parameters.AddWithValue("@PrimaryApproverID", usermanager.PrimaryManagerID);
                cmd.Parameters.AddWithValue("@SecondaryApproverID", usermanager.SecondaryManagerID);
                cmd.Parameters.AddWithValue("@ModUserID", usermanager.ModUserID);
                #endregion

                connection.Open();

                await cmd.ExecuteNonQueryAsync();

                return "Add/Update Successful!";
            }
        }
        #endregion

        #region Get Approver Details
        public async Task<List<GetUserManagerDTO>> GetUserManagerInfoAsync()
        {
            var usermanagers = new List<GetUserManagerDTO>();

            #region Conducting DB Operations
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetUserManagerInfo", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    #region Fetching Data
                    while (reader.Read())
                    {
                        var usermanager = new GetUserManagerDTO
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            ManagerID = reader.GetInt32(reader.GetOrdinal("ManagerID")),
                            ManagerName = reader.GetString(reader.GetOrdinal("ManagerName")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("isPrimary")),
                            IsSecondary = reader.GetBoolean(reader.GetOrdinal("isSecondary"))
                        };

                        if (usermanager.IsPrimary)
                        {
                            usermanagers.Add(new GetUserManagerDTO
                            {
                                UserID = usermanager.UserID,
                                UserName = usermanager.UserName,
                                PrimaryManagerName = usermanager.ManagerName,
                                SecondaryManagerName = null
                            });
                        }
                        else if (usermanager.IsSecondary)
                        {
                            var existinguser = usermanagers.FirstOrDefault(u => u.UserID == usermanager.UserID);
                            if (existinguser != null)
                            {
                                existinguser.SecondaryManagerName = usermanager.ManagerName;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            return (usermanagers);
        }
        #endregion
    }
}
