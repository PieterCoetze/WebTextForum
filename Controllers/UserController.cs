using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebTextForum.Models;
using WebTextForum.Models.Dto_s;
using WebTextForum.Services;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateUser")]
        public IActionResult AuthenticateUser(UserAuthenticateDto userAuthenticateDto)
        {
            User user = _userService.AuthenticateUser(userAuthenticateDto);

            if (user == null)
                return Unauthorized("No user email found.");

            string token = _jwtService.GenerateToken(user);

            return Ok(new { token, user });
        }


        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers(int? userId)
        {
            var response = _userService.GetUsers(userId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
