using Microsoft.AspNetCore.Mvc;
using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface ITaskRepository
    {
        Task<List<GetTaskDTO>> GetAllTasksAsync();
        Task<GetTaskDTO> GetTaskByIdAsync(int id);
         Task<int> AddTask(PostTaskDTO task);
    }
}
