using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTextForum.Models.Dto_s;
using WebTextForum.Services;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IJwtService _jwtService;

        public CommentController(ICommentService commentService, IJwtService jwtService)
        {
            _commentService = commentService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("AddComment")]
        public IActionResult AddComment(CommentDto commentDto)
        {
            var response = _commentService.AddComment(commentDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
