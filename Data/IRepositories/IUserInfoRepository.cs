using UCITMS.Models;

namespace UCITMS.Data.IRepositories
{
    public interface IUserInfoRepository
    {
        UserDTO GetuserbyEmail(string Email);

        //List<int> GetRolesbyuserID(int id);
    }
}
