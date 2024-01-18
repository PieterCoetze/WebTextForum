using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Services.Interfaces
{
    public interface IFlagService
    {
        Response GetFlags();
        Response GetPostFlags(int postId);
        Response PostFlag(PostFlagDto postFlagDto);
    }
}
