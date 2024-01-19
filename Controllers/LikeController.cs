using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;
        private readonly IJwtService _jwtService;

        public LikeController(IJwtService jwtService, ILikeService likeService)
        {
            _likeService = likeService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("LikePost/{postId}")]
        public IActionResult LikePost(int postId)
        {
            var response = _likeService.LikePost(postId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
