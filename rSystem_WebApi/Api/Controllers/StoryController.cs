using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryServices _storyServices;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storyServices">Story Service Call</param>
        public StoryController(IStoryServices storyServices)
        {
            _storyServices = storyServices;
        }
        /// <summary>
        /// Get the strories
        /// </summary>
        /// <param name="page">Page Number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchTerm">searchTerm for storyId</param>
        /// <returns>Response after applying filter</returns>
        [Produces("application/json", "application/xml")]
        [HttpGet]
        public async Task<IActionResult> GetStories(int page=1, int pageSize=10, int searchTerm=0)
        {
            try
            {
                var res = await _storyServices.GetStories(page, pageSize, searchTerm);
                if (res != null)
                {
                    return Ok(res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError , ex.Message);
            }          
            
        }
    }
}
