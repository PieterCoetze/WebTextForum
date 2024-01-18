using WebTextForum.Models;

namespace WebTextForum.Services.Interfaces
{
    public interface IUserService
    {
        Response GetUsers(int? userId);
    }
}
