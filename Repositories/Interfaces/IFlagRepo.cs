using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Repositories.Interfaces
{
    public interface IFlagRepo
    {
        List<Flag> GetFlags();
        List<PostFlag> GetPostFlags(int postId);
        bool CheckIfPostHasFlag(int flagId, int postId);
        bool PostFlag(PostFlagDto postFlagDto, int userId);
    }
}
