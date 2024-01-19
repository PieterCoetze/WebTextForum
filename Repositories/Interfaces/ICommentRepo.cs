using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Repositories.Interfaces
{
    public interface ICommentRepo
    {
        Comment GetComment(int commentId);
        List<Comment> GetCommentsForPost(int postId);
        int AddComment(CommentDto commentDto, int userId);
    }
}
