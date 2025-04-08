using Microsoft.Data.SqlClient;
using System.Data;
using UCITMS.Data.IRepositories;
using UCITMS.ViewModels;

namespace UCITMS.Data.Repositories
{
    public class MenuRepository: IMenuRepository
    {
        private readonly string _connectionString;

        public MenuRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<List<VMMenu>> GetUserMenuById(int userId)
        {
            var menuList = new List<VMMenu>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetUserMenuById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var menu = new VMMenu
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            MenuName = reader.GetString(reader.GetOrdinal("MenuName")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            NavigationPath = reader.GetString(reader.GetOrdinal("NavigationPath")),
                            NavigationType = reader.GetString(reader.GetOrdinal("NavigationType")),
                            SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                            ModifiedBy = reader.GetInt32(reader.GetOrdinal("ModifiedBy"))
                        };
                        menuList.Add(menu);
                    }
                }
            }

            return menuList;
        }
    }
}
