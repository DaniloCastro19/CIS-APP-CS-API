using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly TopicService _TopicService;

        public TopicController(TopicService TopicService)
        {
            _TopicService = TopicService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
        {
            var Topics = await _TopicService.GetAllTopics();
            return Ok(Topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopicById(string id)
        {
            var Topic = await _TopicService.GetTopicById(id);
            if (Topic == null)
            {
                return NotFound();
            }
            return Ok(Topic);
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicDTO TopicDTO)
        {
            var newTopic = TopicDTO.ToCompleteTopic(TopicDTO);
            var createdTopic = await _TopicService.CreateTopic(newTopic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] TopicDTO TopicDTO)
        {
            var updatedTopic = TopicDTO.ToCompleteTopic(TopicDTO);
            updatedTopic.Id = id;
            try
            {
                await _TopicService.UpdateTopic(updatedTopic);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(string id)
        {
            var Topic = await _TopicService.DeleteTopic(id);
            if (Topic == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}