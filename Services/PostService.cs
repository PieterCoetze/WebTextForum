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

        public PostService(IUnitOfWork unitOfWork, IJwtService jwtService, ICommentService commentService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _commentService = commentService;
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
                    post.Likes = _unitOfWork.PostRepo.GetPostLikeCount(post.PostId);
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

        public Response LikePost(int postId)
        {
            Response response = new Response() { Message = "Successfully liked post.", Success = true };

            try
            {
                int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

                if(_unitOfWork.PostRepo.HasLikedPost(postId, userId))
                {
                    response.Data = _unitOfWork.PostRepo.RemoveLike(postId, userId);
                    response.Message = "Successfully removed like.";
                }
                else
                {
                    response.Data = _unitOfWork.PostRepo.AddLike(postId, userId);
                }

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                response.Message = "Unable to like post.";
                response.Success = false;
            }

            return response;
        }
    }
}
