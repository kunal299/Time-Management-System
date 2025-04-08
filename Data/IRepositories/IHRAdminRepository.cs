using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface IHRAdminRepository
    {
        Task<List<GetUserManagerDTO>> GetUserManagerInfoAsync();

        Task<string> AddOrUpdateApproversAsync(PostUserManagerDTO usermanager);
    }
}
