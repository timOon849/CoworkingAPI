using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingAPI.Controllers
{
    [Route("api/spaces/{spaceId}/workplaces")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        private readonly ISpaceService _spaceService;

        public WorkplaceController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkplacesBySpaceId(int spaceId)
        {
            var workplaces = await _spaceService.GetWorkplacesBySpaceId(spaceId);
            return Ok(workplaces);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkplace(int spaceId, [FromBody] Workplace workplace)
        {
            try
            {
                var createdWorkplace = await _spaceService.AddWorkplaceToSpace(spaceId, workplace);
                return CreatedAtAction(nameof(GetWorkplacesBySpaceId), new { spaceId }, createdWorkplace);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{workplaceId}")]
        public async Task<IActionResult> DeleteWorkplace(int workplaceId)
        {
            var isDeleted = await _spaceService.DeleteWorkplace(workplaceId);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
