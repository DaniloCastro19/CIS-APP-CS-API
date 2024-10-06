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
        public async Task<IActionResult> CreateVote(Guid ideaId, [FromBody] VoteDto voteDto)
        {
            try
            {
                _voteDTOvalidator.ValidateAndThrow(voteDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createVote = await _voteService.Create(voteDto, userId, ideaId);
            return CreatedAtAction(nameof(GetVoteById), new { id = createVote.Id }, createVote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVote(Guid id, [FromBody] VoteDto voteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVote = await _voteService.GetByID(id);
            if (existingVote == null)
            {
                return NotFound();
            }

            existingVote.IsPositive = voteDto.IsPositive;

            await _voteService.Update(existingVote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(Guid id)
        {
            try
            {
                await _voteService.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
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