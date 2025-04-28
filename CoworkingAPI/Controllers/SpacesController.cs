using CoworkingAPI.Interfaces;
using CoworkingAPI.Model;
using CoworkingAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingAPI.Controllers
{
    [Route("api/spaces")]
    [ApiController]
    public class SpaceController : ControllerBase
    {
        private readonly ISpaceService _spaceService;

        public SpaceController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpaceById(int id)
        {
            var space = await _spaceService.GetSpaceById(id);
            if (space == null)
                return NotFound();

            return Ok(space);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpaces([FromQuery] SpaceFilter filter, [FromQuery] Pagination pagination)
        {
            var spaces = await _spaceService.GetAllSpaces(filter, pagination);
            return Ok(spaces);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpace([FromBody] Space space)
        {
            var createdSpace = await _spaceService.CreateSpace(space);
            return CreatedAtAction(nameof(GetSpaceById), new { id = createdSpace.Id }, createdSpace);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpace(int id, [FromBody] Space space)
        {
            try
            {
                var updatedSpace = await _spaceService.UpdateSpace(id, space);
                return Ok(updatedSpace);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpace(int id)
        {
            var isDeleted = await _spaceService.DeleteSpace(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{spaceId}/workplacesAdd")]
        public async Task<IActionResult> AddWorkplaceToSpace(int spaceId, [FromBody] Workplace workplace)
        {
            try
            {
                var createdWorkplace = await _spaceService.AddWorkplaceToSpace(spaceId, workplace);
                return CreatedAtAction(nameof(GetWorkplacesBySpaceId), new { id = createdWorkplace.Id }, createdWorkplace);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{spaceId}/workplacesById")]
        public async Task<IActionResult> GetWorkplacesBySpaceId(int spaceId)
        {
            var workplaces = await _spaceService.GetWorkplacesBySpaceId(spaceId);
            return Ok(workplaces);
        }
    }
}

