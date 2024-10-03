using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _voteService;

        public VoteController(VoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVotes()
        {
            var votes = await _voteService.GetAllVotes();
            return Ok(votes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoteById(Guid id)
        {
            var vote = await _voteService.GetVoteById(id);
            if (vote == null)
            {
                return NotFound();
            }
            return Ok(vote);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVote([FromBody] VoteDto voteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newVote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                IdeasId = voteDto.IdeasId,
                UsersId = voteDto.UsersId,
                IsPositive = voteDto.IsPositive,
            };

            var createdVote = await _voteService.CreateVote(newVote);
            return CreatedAtAction(nameof(GetVoteById), new { id = createdVote.Id }, createdVote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVote(Guid id, [FromBody] VoteDto voteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVote = await _voteService.GetVoteById(id);
            if (existingVote == null)
            {
                return NotFound();
            }

            existingVote.IsPositive = voteDto.IsPositive;

            await _voteService.UpdateVote(existingVote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(Guid id)
        {
            var vote = await _voteService.DeleteVote(id);
            if (vote == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetVotesByUserId(string userId)
        {
            var votes = await _voteService.GetVotesByUserId(userId);
            return Ok(votes);
        }

        [HttpGet("idea/{ideaId}")]
        public async Task<IActionResult> GetVotesByIdeaId(string ideaId)
        {
            var votes = await _voteService.GetVotesByIdeaId(ideaId);
            return Ok(votes);
        }

        [HttpGet("idea/{ideaId}/positive/count")]
        public async Task<IActionResult> CountPositiveVotesByIdeaId(string ideaId)
        {
            var count = await _voteService.CountPositiveVotesByIdeaId(ideaId);
            return Ok(count);
        }

        [HttpGet("idea/{ideaId}/negative/count")]
        public async Task<IActionResult> CountNegativeVotesByIdeaId(string ideaId)
        {
            var count = await _voteService.CountNegativeVotesByIdeaId(ideaId);
            return Ok(count);
        }
    }
}