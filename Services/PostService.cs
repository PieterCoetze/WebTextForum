using System.Security.Claims;
using WebTextForum.Models;
using WebTextForum.Models.Dto_s;
using WebTextForum.Repositories.UnitOfWork;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public PostService(IUnitOfWork unitOfWork, IJwtService jwtService, ICommentService commentService, ILikeService likeService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _commentService = commentService;
            _likeService = likeService;
        }

        public Response GetPosts(int? postId)
        {
            Response response = new Response() { Message = "Successfully retrieved posts.", Success = true };

            try
            {
                response.Data = _unitOfWork.PostRepo.GetPosts(postId);

                foreach (Post post in response.Data)
                {
                    post.Comments = _commentService.GetCommentsForPost(post.PostId).ToArray();
                    post.Likes = _likeService.GetPostLikeCount(post.PostId);
                }
            }
            catch (Exception ex)
            {
                response.Message = "Unable to retrieve posts.";
                response.Success = false;
            }

            return response;
        }

        public Response AddPost(PostDto postDto)
        {
            Response response = new Response() { Message = "Successfully added post.", Success = true };

            try
            {
                int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

                response.Data = _unitOfWork.PostRepo.AddPost(postDto, userId);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                response.Message = "Unable to add post.";
                response.Success = false;
            }

            return response;
        }

        public int GetPostOwner(int postId)
        {
            int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

            return _unitOfWork.PostRepo.GetPostOwner(postId);
        }
    }
}
