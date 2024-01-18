using WebTextForum.Models;

namespace WebTextForum.Services.Interfaces
{
    public interface ILikeService
    {
        int GetPostLikeCount(int postId);
        bool HasLikedPost(int postId);
        Response LikePost(int postId);
    }
}
