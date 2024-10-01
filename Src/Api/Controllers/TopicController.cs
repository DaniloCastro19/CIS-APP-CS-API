using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<Topic>> GetTopicById(Guid id) 
        {
            var Topic = await _TopicService.GetTopicById(id);
            if (Topic == null)
            {
                return NotFound();
            }
            return Ok(Topic);
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicDTO topicDTO)
        {
            var newTopic = new Topic
            {
                Id = Guid.NewGuid().ToString(), 
                Title = topicDTO.Title,
                Description = topicDTO.Description,
                CreationDate = DateTime.UtcNow, 
                UsersId = topicDTO.UsersId 
            };

            var createdTopic = await _TopicService.CreateTopic(newTopic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] UpdateTopicDTO topicDTO) 
        {
            if (!Guid.TryParse(id, out Guid topicId))
            {
                return BadRequest("Invalid UUID format");
            }
            var existingTopic = await _TopicService.GetTopicById(topicId);
            if (existingTopic == null)
            {
                return NotFound();
            }
            existingTopic.Title = topicDTO.Title ?? existingTopic.Title;
            existingTopic.Description = topicDTO.Description ?? existingTopic.Description;
            existingTopic.CreationDate = topicDTO.CreationDate ?? existingTopic.CreationDate;
            try
            {
                await _TopicService.UpdateTopic(existingTopic);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return Ok(existingTopic);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id) 
        {
            var topic = await _TopicService.DeleteTopic(id); 
            if (topic == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}