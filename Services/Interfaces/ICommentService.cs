using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Services.Interfaces
{
    public interface ICommentService
    {
        List<Comment> GetCommentsForPost(int postId);
        Response AddComment(CommentDto commentDto);
    }
}
