using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
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
