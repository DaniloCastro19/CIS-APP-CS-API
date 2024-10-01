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
        private readonly TopicService _topicService;

        public TopicController(TopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
        {
            var topics = await _topicService.GetAllTopics();
            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopicById(Guid id) 
        {
            var topic = await _topicService.GetTopicById(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(topic);
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

            var createdTopic = await _topicService.CreateTopic(newTopic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] TopicDTO topicDTO) 
        {
            var updatedTopic = new Topic
            {
                Id = id, 
                Title = topicDTO.Title,
                Description = topicDTO.Description,
                CreationDate = DateTime.UtcNow,
                UsersId = topicDTO.UsersId 
            };

            try
            {
                await _topicService.UpdateTopic(updatedTopic);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id) 
        {
            var topic = await _topicService.DeleteTopic(id); 
            if (topic == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}