using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdeaController : ControllerBase
    {
        private readonly IdeaService _ideaService;

        public IdeaController(IdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ideas = await _ideaService.GetAllIdeas();
            return Ok(ideas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var idea = await _ideaService.GetIdeaById(id);
            if (idea == null)
            {
                return NotFound();
            }
            return Ok(idea);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IdeaDTO ideaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newIdea = new Idea
            {
                Id = Guid.NewGuid().ToString(),
                Content = ideaDto.Content,
                CreationDate = DateTime.UtcNow,
                UsersId = ideaDto.UsersId,
                TopicsId = ideaDto.TopicsId
            };

            await _ideaService.CreateIdea(newIdea);
            return CreatedAtAction(nameof(GetById), new { id = newIdea.Id }, newIdea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] IdeaDTO ideaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingIdea = await _ideaService.GetIdeaById(id);
            if (existingIdea == null)
            {
                return NotFound();
            }

            existingIdea.Content = ideaDto.Content;
            existingIdea.TopicsId = ideaDto.TopicsId;

            await _ideaService.UpdateIdea(existingIdea);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var idea = await _ideaService.DeleteIdea(id);
            if (idea == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}