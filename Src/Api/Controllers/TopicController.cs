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
<<<<<<< HEAD
        public async Task<ActionResult<Topic>> GetTopicById(string id)
=======
        public async Task<ActionResult<Topic>> GetTopicById(Guid id) 
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
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
<<<<<<< HEAD
            var newTopic = TopicDTO.ToCompleteTopic(TopicDTO);
            var createdTopic = await _TopicService.CreateTopic(newTopic);
=======
            var newTopic = new Topic
            {
                Id = Guid.NewGuid().ToString(), 
                Title = topicDTO.Title,
                Description = topicDTO.Description,
                CreationDate = DateTime.UtcNow, 
                UsersId = topicDTO.UsersId 
            };

            var createdTopic = await _topicService.CreateTopic(newTopic);
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        [HttpPut("{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] TopicDTO TopicDTO)
        {
            var updatedTopic = TopicDTO.ToCompleteTopic(TopicDTO);
            updatedTopic.Id = id;
=======
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

>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
            try
            {
                await _TopicService.UpdateTopic(updatedTopic);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> DeleteTopic(string id)
        {
            var Topic = await _TopicService.DeleteTopic(id);
            if (Topic == null)
=======
        public async Task<IActionResult> DeleteTopic(Guid id) 
        {
            var topic = await _topicService.DeleteTopic(id); 
            if (topic == null)
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}