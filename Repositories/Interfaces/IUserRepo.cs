using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Repositories.Interfaces
{
    public interface IUserRepo
    {
        User AuthenticateUser(UserAuthenticateDto userAuthenticateDto);
        List<User> GetUsers(int? userId);
    }
}
