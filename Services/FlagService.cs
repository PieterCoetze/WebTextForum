using System.Security.Claims;
using WebTextForum.Models.Dto_s;
using WebTextForum.Models;
using WebTextForum.Repositories.UnitOfWork;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class FlagService : IFlagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public FlagService(IUnitOfWork unitOfWork, IJwtService jwtService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _userService = userService;
        }

        public Response GetFlags()
        {
            Response response = new Response() { Message = "Successfully retrieved flag.", Success = true };

            try
            {
                response.Data = _unitOfWork.FlagRepo.GetFlags();
            }
            catch (Exception ex)
            {
                response.Message = "Unable to retrieve flag.";
                response.Success = false;
            }

            return response;
        }

        public Response GetPostFlags(int postId)
        {
            Response response = new Response() { Message = "Successfully retrieved data.", Success = true };

            try
            {
                response.Data = _unitOfWork.FlagRepo.GetPostFlags(postId);
            }
            catch (Exception ex)
            {
                response.Message = "Unable to retrieved data.";
                response.Success = false;
            }

            return response;
        }

        public Response PostFlag(PostFlagDto postFlagDto)
        {
            Response response = new Response() { Message = "Successfully added flag.", Success = true };

            try
            {
                int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

                if (_userService.GetUserType(userId).Code == "MOD")
                {
                    if(_unitOfWork.FlagRepo.CheckIfPostHasFlag(postFlagDto.FlagId, postFlagDto.PostId))
                    {
                        response.Message = $"This post has already been flagged with {postFlagDto.FlagId}.";
                    }
                    else
                    {
                        response.Data = _unitOfWork.FlagRepo.PostFlag(postFlagDto, userId);

                        _unitOfWork.Commit();
                    }
                }
                else
                {
                    response.Message = "User must be a moderator to flag a post.";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Unable to add flag.";
                response.Success = false;
            }

            return response;
        }
    }
}
