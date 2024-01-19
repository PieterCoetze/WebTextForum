using WebTextForum.Models.Dto_s;
using WebTextForum.Models;

namespace WebTextForum.Repositories.Interfaces
{
    public interface IPostRepo
    {
        List<Post> GetPosts(GetPostDto getPostDto);
        int GetPostOwner(int postId);
        int AddPost(PostDto postDto, int userId);
    }
}
