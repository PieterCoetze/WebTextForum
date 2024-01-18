namespace WebTextForum.Repositories.Interfaces
{
    public interface ILikeRepo
    {
        bool HasLikedPost(int postId, int userId);
        bool AddLike(int postId, int userId);
        bool RemoveLike(int postId, int userId);
        int GetPostLikeCount(int postId);
    }
}
