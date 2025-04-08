using UCITMS.ViewModels;

namespace UCITMS.Data.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<VMMenu>> GetUserMenuById(int userId);
    }
}
