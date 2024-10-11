using System.Security.Claims;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdeaController : ControllerBase
    {
        private readonly IIdeaService _ideaService;

        public IdeaController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool mostWanted= false)
        {
            var ideas = await _ideaService.GetAll(mostWanted);
            return Ok(ideas);
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var ideas = await _ideaService.GetByUser(userId);
            return Ok(ideas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var idea = await _ideaService.GetByID(id);
            if (idea == null)
            {
                return NotFound();
            }
            return Ok(idea);
        }

        [HttpPost("{topicId}")]
        public async Task<ActionResult<Idea>> Create(Guid topicId, [FromBody] IdeaDTO ideaDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newIdea = await _ideaService.Create(ideaDto, userId, topicId);
            return Ok(newIdea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] IdeaDTO ideaDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _ideaService.Update(id, ideaDto,userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ObjectResult(new { message = ex.Message })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok("Idea Updated Succesfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try{
                await _ideaService.Delete(id, userId);
            }catch (UnauthorizedAccessException ex)
            {
                return new ObjectResult(new { message = ex.Message })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok("Idea Deleted Succesfully.");
        }
    }
}