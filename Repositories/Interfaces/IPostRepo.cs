using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Repositories.Interfaces
{
    public interface IPostRepo
    {
        List<Post> GetPosts(int? postId);
        Post AddPost(PostDto postDto, int userId);
    }
}
