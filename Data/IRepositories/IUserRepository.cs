using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetUsersUnderManager(int managerId);
    }
}
