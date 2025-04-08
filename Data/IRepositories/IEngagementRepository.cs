using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface IEngagementRepository
    {
        Task<int> SaveEngagementAsync(PostEngagementDTO engagement);

        Task<List<GetEngagementDTO>> GetAllEngagements();

        GetEngagementDTO GetEngagementById(int engagementId);

        Task<List<GetEngagementDTO>> GetEngagementsByUserIdAsync(int? userId);

        Task<List<OwnerDTO>> GetProjectOwnersAsync();

        List<GetEngagementDTO> GetEngagementsByUserAndDate(int userId, DateTime date);


        Task<IEnumerable<GetAllEngagementsDTO>> GetEngagementsforEmployeeAsync(int? id);
    }
}
