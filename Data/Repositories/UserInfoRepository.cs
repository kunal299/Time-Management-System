using Microsoft.Data.SqlClient;
using System.Data;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Data.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly string _connectionString;

        public UserInfoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UserDTO GetuserbyEmail(string Email)
        {
            UserDTO model = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "GetUserInfo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", Email);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model = new UserDTO
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = reader["DisplayName"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }

            }

            return model;
        }
    }
}
