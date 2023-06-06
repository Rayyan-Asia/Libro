using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/test")]
    public class TestJwtTokenExtractorController : Controller
    {
        private readonly IConfiguration _config;
        public TestJwtTokenExtractorController(IConfiguration config)
        {

            _config = config;

        }
        [HttpGet]
        public IActionResult Index([FromHeader] string authentication)
        {
            var user = TokenHandler.GetUserFromPayload(authentication);

            return Ok(user);
            
        }
    }
}
