using System.Security.Claims;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private IValidator<TopicDTO> _topicDTOvalidator;

        public TopicController(ITopicService topicService, IValidator<TopicDTO> validator)
        {
            _topicService = topicService;
            _topicDTOvalidator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
        {
            var topics = await _topicService.GetAll();
            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopicById(Guid id) 
        {
            var topic = await _topicService.GetByID(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(topic);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetByUser(Guid id)
        {
            var topics = await _topicService.GetByUser(id);
            if (topics == null)
            {
                return NotFound();
            }
            return Ok(topics);
        }

        [HttpPost]
        public async Task<ActionResult<TopicDTOResponse>> CreateTopic([FromBody] TopicDTO topicDTO)
        {
            try
            {
                _topicDTOvalidator.ValidateAndThrow(topicDTO);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var response = await _topicService.Create(topicDTO, userId);
            if (response== null) return BadRequest("Something goes wrong."); 

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] TopicDTO topicDTO) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!Guid.TryParse(id, out Guid topicId))
            {
                return BadRequest(new { message = "Invalid topic ID format." });
            }
            try
            {
                await _topicService.Update(topicDTO, userId, topicId);
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

            return Ok("Topic Updated Succesfully.");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                await _topicService.Delete(id, userId);
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

            return Ok("Topic Deleted Succesfully.");

        }
    }
}