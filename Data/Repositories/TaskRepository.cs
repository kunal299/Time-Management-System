using Microsoft.Data.SqlClient;
using System.Data;
using UCITMS.Data.IRepositories;
using UCITMS.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UCITMS.Data.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Retrieve all tasks
        public async Task<List<GetTaskDTO>> GetAllTasksAsync()
        {
            var tasks = new List<GetTaskDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT TaskID, TaskName, TaskDescription, IsDeleted, IsGeneric, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn FROM EngagementTasks WHERE IsDeleted = 0";

                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var task = new GetTaskDTO
                        {
                            TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                            TaskName = reader.GetString(reader.GetOrdinal("TaskName")),
                            TaskDescription = reader.GetString(reader.GetOrdinal("TaskDescription")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            IsGeneric = reader.GetBoolean(reader.GetOrdinal("IsGeneric")),
                        };
                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        // Retrieve a task by ID
        public async Task<GetTaskDTO> GetTaskByIdAsync(int id)
        {
            GetTaskDTO task = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT TaskID, TaskName, TaskDescription, IsDeleted, IsGeneric, CreatedBy, ModifiedBy, CreatedOn, ModifiedOn FROM EngagementTasks WHERE TaskID = @TaskID AND IsDeleted = 0";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TaskID", id);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        task = new GetTaskDTO
                        {
                            TaskID = reader.GetInt32(reader.GetOrdinal("TaskID")),
                            TaskName = reader.GetString(reader.GetOrdinal("TaskName")),
                            TaskDescription = reader.GetString(reader.GetOrdinal("TaskDescription")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            IsGeneric = reader.GetBoolean(reader.GetOrdinal("IsGeneric")),
                        };
                    }
                }
            }

            return task;
        }

        public async Task<int> AddTask(PostTaskDTO task)
        {
            int taskId = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.AddTask", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TaskName", task.TaskName);
                    command.Parameters.AddWithValue("@TaskDescription", task.TaskDescription);
                    command.Parameters.AddWithValue("@IsDeleted", task.IsDeleted);
                    command.Parameters.AddWithValue("@IsGeneric", task.IsGeneric);
                    command.Parameters.AddWithValue("@ModUser", task.ModUser);

                    SqlParameter outputIdParam = new SqlParameter("@TaskID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    await conn.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    taskId = Convert.ToInt32(outputIdParam.Value);
                }
            }

            return taskId;
        }
    }
}
