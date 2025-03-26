using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryServices _storyServices;
        public StoryController(IStoryServices storyServices)
        {
            _storyServices = storyServices;
        }

        [Produces("application/json", "application/xml")]
        [HttpGet]
        public async Task<IActionResult> GetStories(int page=1, int pageSize=10)
        {
            try
            {
                var res = await _storyServices.GetStories(page, pageSize);
                if (res != null)
                {
                    return Ok(res);
                }
                return StatusCode(StatusCodes.Status204NoContent, "No stories found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError , ex.Message);
            }
            
            
        }
    }
}
