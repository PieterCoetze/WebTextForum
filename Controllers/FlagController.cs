using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTextForum.Models.Dto_s;
using WebTextForum.Services.Interfaces;

namespace WebTextForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FlagController : Controller
    {
        private readonly IFlagService _flagService;
        private readonly IJwtService _jwtService;

        public FlagController(IJwtService jwtService, IFlagService flagService)
        {
            _flagService = flagService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("GetFlags")]
        public IActionResult GetFlags()
        {
            var response = _flagService.GetFlags();

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost]
        [Route("PostFlag")]
        public IActionResult PostFlag(PostFlagDto postFlagDto)
        {
            var response = _flagService.PostFlag(postFlagDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
