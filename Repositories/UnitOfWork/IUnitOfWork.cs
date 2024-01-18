using System.Data;
using WebTextForum.Repositories.Interfaces;

namespace WebTextForum.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepo UserRepo { get; set; }
        IPostRepo PostRepo { get; set; }
        ICommentRepo CommentRepo { get; set; }
        void Commit();
        void Dispose();
    }
}
