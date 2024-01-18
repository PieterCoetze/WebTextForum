using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTextForum.Models.Dto_s;
using WebTextForum.Models;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IJwtService _jwtService;

        public PostController(IJwtService jwtService, IPostService postService)
        {
            _postService = postService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetPosts")]
        public IActionResult GetPosts(int? postId)
        {
            var response = _postService.GetPosts(postId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddPost")]
        public IActionResult AddPost(PostDto postDto)
        {
            var response = _postService.AddPost(postDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

       
    }
}
