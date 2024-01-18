using WebTextForum.Models;


namespace WebTextForum.Repositories.Interfaces
{
    public interface IUserRepo
    {
        List<User> GetUsers(int? userId);
    }
}
