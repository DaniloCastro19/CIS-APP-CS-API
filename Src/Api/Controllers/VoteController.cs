using System.Security.Claims;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private IValidator<VoteDto> _voteDTOvalidator;

        public VoteController(IVoteService voteService, IValidator<VoteDto> voteValidator)
        {
            _voteService = voteService;
            _voteDTOvalidator = voteValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVotes()
        {
            var votes = await _voteService.GetAll();
            return Ok(votes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoteById(Guid id)
        {
            var vote = await _voteService.GetByID(id);
            if (vote == null)
            {
                return NotFound();
            }
            return Ok(vote);
        }

        [HttpPost("idea/{ideaId}")]
        public async Task<ActionResult<Idea>> CreateVote(Guid ideaId, [FromHeader] bool voteValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdVote = await _voteService.Create(voteValue, userId, ideaId);
            return Ok(createdVote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVote(Guid id, [FromHeader] bool voteValue)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _voteService.Update(id, voteValue,userId);
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
            return Ok("Vote updated suceesfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                await _voteService.Delete(id, userId);
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
            return Ok("Vote deleted suceesfully");

        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetVotesByUserId(Guid userId)
        {
            var votes = await _voteService.GetVotesByUserId(userId);
            return Ok(votes);
        }

        [HttpGet("idea/{ideaId}")]
        public async Task<IActionResult> GetVotesByIdeaId(Guid ideaId)
        {
            var votes = await _voteService.GetVotesByIdeaId(ideaId);
            return Ok(votes);
        }

        [HttpGet("idea/{ideaId}/positive/count")]
        public async Task<IActionResult> CountPositiveVotesByIdeaId(Guid ideaId)
        {
            var count = await _voteService.CountPositiveVotesByIdeaId(ideaId);
            return Ok(count);
        }

        [HttpGet("idea/{ideaId}/negative/count")]
        public async Task<IActionResult> CountNegativeVotesByIdeaId(Guid ideaId)
        {
            var count = await _voteService.CountNegativeVotesByIdeaId(ideaId);
            return Ok(count);
        }
    }
}