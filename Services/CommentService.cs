using System.Security.Claims;
using WebTextForum.Models.Dto_s;
using WebTextForum.Models;
using WebTextForum.Repositories.UnitOfWork;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public CommentService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public List<Comment> GetCommentsForPost(int postId)
        {
            return _unitOfWork.CommentRepo.GetCommentsForPost(postId);
        }

        public Response AddComment(CommentDto commentDto)
        {
            Response response = new Response() { Message = "Successfully retrieved data.", Success = true };

            try
            {
                int userId = int.Parse(_jwtService?.GetClaim(ClaimTypes.NameIdentifier));

                response.Data = _unitOfWork.CommentRepo.AddComment(commentDto, userId);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                response.Message = "Unable to retrieve data.";
                response.Success = false;
            }

            return response;
        }
    }
}
