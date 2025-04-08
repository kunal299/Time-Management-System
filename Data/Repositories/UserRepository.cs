using Microsoft.Data.SqlClient;
using UCITMS.Data.IRepositories;
using UCITMS.Models;

namespace UCITMS.Data.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var userList = new List<UserDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users", conn);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new UserDTO
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Username = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                        userList.Add(user);
                    }
                }
            }

            return userList;
        }

        public async Task<List<UserDTO>> GetUsersUnderManager(int managerId)
        {
            var userList = new List<UserDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetUsersUnderManager", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ManagerID", managerId);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new UserDTO
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Username = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                        userList.Add(user);
                    }
                }
            }

            return userList;
        }
    }
}
