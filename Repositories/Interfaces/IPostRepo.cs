using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Repositories.Interfaces
{
    public interface IPostRepo
    {
        List<Post> GetPosts(int? postId);
        Post AddPost(PostDto postDto, int userId);
        bool HasLikedPost(int postId, int userId);
        bool AddLike(int postId, int userId);
        bool RemoveLike(int postId, int userId);
        int GetPostLikeCount(int postId);
    }
}
