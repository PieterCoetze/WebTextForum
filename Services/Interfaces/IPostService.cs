using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Services.Interfaces
{
    public interface IPostService
    {
        Response GetPosts(GetPostDto getPostDto);
        Response AddPost(PostDto postDto);
        int GetPostOwner(int postId);
    }
}
