using WebTextForum.Models;
using WebTextForum.Models.Dto_s;
using WebTextForum.Repositories.UnitOfWork;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User AuthenticateUser(UserAuthenticateDto userAuthenticateDto)
        {
            return _unitOfWork.UserRepo.AuthenticateUser(userAuthenticateDto);
        }

        public Response GetUsers(int? userId)
        {
            Response response = new Response() { Message = "Successfully retrieved data.", Success = true };

            try
            {
                response.Data = _unitOfWork.UserRepo.GetUsers(userId);
            }
            catch (Exception ex)
            {
                response.Message = "Unable to retrieve data.";
                response.Success = false;
            }

            return response;
        }

        public UserType GetUserType(int userId)
        {
            return _unitOfWork.UserRepo.GetUserType(userId);
        }
    }
}
