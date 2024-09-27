using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController:ControllerBase
    {
        private readonly IRepositoryGeneric<Topic> _repository;

        public TopicController(IRepositoryGeneric<Topic> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var data = await _repository.GetAll();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicById(Guid id)
        {
            var topic = await _repository.GetByID(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(topic);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTopic([FromBody] TopicDTO newTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTopic = TopicDTO.ToCompleteTopic(newTopicDto);

            var createdTopic = await _repository.Insert(newTopic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTopic(Guid id, [FromBody] TopicDTO updatedTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedTopic = TopicDTO.ToCompleteTopic(updatedTopicDto);
            updatedTopic.Id = id;
            try
            {
                await _repository.Update(updatedTopic);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(Guid id)
        {
            var topic = await _repository.GetByID(id);
            if (topic == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);
            return NoContent();
        }
    }

}