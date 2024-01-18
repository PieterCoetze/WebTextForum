using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Services.Interfaces
{
    public interface IPostService
    {
        Response GetPosts(int? postId);
        Response AddPost(PostDto postDto);
    }
}
