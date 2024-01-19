using System.Security.Claims;
using WebTextForum.Models.Dto_s;
using WebTextForum.Models;
using WebTextForum.Repositories.UnitOfWork;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public LikeService(IUnitOfWork unitOfWork, IJwtService jwtService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _userService = userService;
        }

        public int GetPostLikeCount(int postId)
        {
            return _unitOfWork.LikeRepo.GetPostLikeCount(postId);
        }

        public bool HasLikedPost(int postId)
        {
            int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

            return _unitOfWork.LikeRepo.HasLikedPost(postId, userId);
        }

        public Response LikePost(int postId)
        {
            Response response = new Response() { Message = "Successfully liked post.", Success = true };

            try
            {
                int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

                if (_userService.GetUserType(userId).Code == "MOD")
                {
                    response.Message = "Moderators cannot like posts.";
                }
                else if (userId == _unitOfWork.PostRepo.GetPostOwner(postId))
                {
                    response.Message = "Users cannot like their own posts.";
                }
                else
                {
                    if (_unitOfWork.LikeRepo.HasLikedPost(postId, userId))
                    {
                        response.Data = _unitOfWork.LikeRepo.RemoveLike(postId, userId);
                        response.Message = "Successfully removed like.";
                    }
                    else
                    {
                        response.Data = _unitOfWork.LikeRepo.AddLike(postId, userId);
                    }

                    _unitOfWork.Commit();
                }
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
