using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Repositories.Interfaces
{
    public interface ICommentRepo
    {
        List<Comment> GetCommentsForPost(int postId);
        Comment? AddComment(CommentDto commentDto, int userId);
    }
}
