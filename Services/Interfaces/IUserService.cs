using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Services.Interfaces
{
    public interface IUserService
    {
        User AuthenticateUser(UserAuthenticateDto userAuthenticateDto);
        Response GetUsers(int? userId);
    }
}
