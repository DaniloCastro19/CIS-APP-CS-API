using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cis_api_legacy_integration_phase_2.Test.Api.Controllers
{
    public class VoteControllerTests
    {
        private readonly Mock<IVoteService> _voteServiceMock;
        private readonly VoteController _controller;

        public VoteControllerTests()
        {
            _voteServiceMock = new Mock<IVoteService>();
            _controller = new VoteController(_voteServiceMock.Object);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
            };
        }

        [Fact]
        public async Task GetAllVotes_ShouldReturnOkResult_WithVotes()
        {
            var votes = new List<Vote>
            {
                new Vote { Id = "1", IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = "2", IdeasId = "idea2", IdeaName = "Name 2", UsersId = "user2", OwnerLogin = "user2", IsPositive = false }
            };
            _voteServiceMock.Setup(v => v.GetAll()).ReturnsAsync(votes);
            var result = await _controller.GetAllVotes();
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVotes = Assert.IsAssignableFrom<IEnumerable<Vote>>(okResult.Value);
            Assert.Equal(2, returnedVotes.Count());
        }

        [Fact]
        public async Task GetVoteById_ShouldReturnOkResult_WithVote()
        {
            var voteId = Guid.NewGuid();
            var vote = new Vote { Id = voteId.ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true };
            _voteServiceMock.Setup(v => v.GetByID(voteId)).ReturnsAsync(vote);
            
            var result = await _controller.GetVoteById(voteId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVote = Assert.IsType<Vote>(okResult.Value);
            Assert.Equal(vote.Id, returnedVote.Id);
        }
        

        [Fact]
        public async Task UpdateVote_ShouldReturnOkResult_WhenVoteUpdated()
        {
            var voteId = Guid.NewGuid();
            var voteValue = true;
            _voteServiceMock.Setup(v => v.Update(voteId, voteValue, It.IsAny<string>()));
            
            var result = await _controller.UpdateVote(voteId, voteValue);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Vote updated suceesfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteVote_ShouldReturnOkResult_WhenVoteDeleted()
        {
            var voteId = Guid.NewGuid();
            _voteServiceMock.Setup(v => v.Delete(voteId, It.IsAny<string>()));
            
            var result = await _controller.DeleteVote(voteId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Vote deleted suceesfully", okResult.Value);
        }

        [Fact]
        public async Task GetVotesByUserId_ShouldReturnOkResult_WithVotes()
        {
            var userId = Guid.NewGuid();
            var votes = new List<Vote>
            {
                new Vote { Id = "1", IdeasId = "idea1", IdeaName = "Name 1", UsersId = userId.ToString(), OwnerLogin = "user1", IsPositive = true }
            };
            _voteServiceMock.Setup(v => v.GetVotesByUserId(userId)).ReturnsAsync(votes);
            
            var result = await _controller.GetVotesByUserId(userId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVotes = Assert.IsAssignableFrom<IEnumerable<Vote>>(okResult.Value);
            Assert.Single(returnedVotes);
        }

        [Fact]
        public async Task GetVotesByIdeaId_ShouldReturnOkResult_WithVotes()
        {
            var ideaId = Guid.NewGuid();
            var votes = new List<Vote>
            {
                new Vote { Id = "1", IdeasId = ideaId.ToString(), IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true }
            };
            _voteServiceMock.Setup(v => v.GetVotesByIdeaId(ideaId)).ReturnsAsync(votes);
            
            var result = await _controller.GetVotesByIdeaId(ideaId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVotes = Assert.IsAssignableFrom<IEnumerable<Vote>>(okResult.Value);
            Assert.Single(returnedVotes);
        }

        [Fact]
        public async Task CountPositiveVotesByIdeaId_ShouldReturnOkResult_WithCount()
        {
            var ideaId = Guid.NewGuid();
            var count = 5;
            _voteServiceMock.Setup(v => v.CountPositiveVotesByIdeaId(ideaId)).ReturnsAsync(count);
            
            var result = await _controller.CountPositiveVotesByIdeaId(ideaId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(count, okResult.Value);
        }

        [Fact]
        public async Task CountNegativeVotesByIdeaId_ShouldReturnOkResult_WithCount()
        {
            var ideaId = Guid.NewGuid();
            var count = 2;
            _voteServiceMock.Setup(v => v.CountNegativeVotesByIdeaId(ideaId)).ReturnsAsync(count);
            
            var result = await _controller.CountNegativeVotesByIdeaId(ideaId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(count, okResult.Value);
        }
        
        //Negative test
        [Fact]
        public async Task GetVoteById_ShouldReturnNotFound_WhenVoteDoesNotExist()
        {
            var voteId = Guid.NewGuid();
            _voteServiceMock.Setup(v => v.GetByID(voteId)).ReturnsAsync((Vote)null);

            var result = await _controller.GetVoteById(voteId);
            
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
        
        
        [Fact]
        public async Task UpdateVote_ShouldReturnNotFound_WhenVoteDoesNotExist()
        {
            var voteId = Guid.NewGuid();
            var voteValue = true;
            var userId = "test_user_id";

            _voteServiceMock.Setup(v => v.Update(voteId, voteValue, userId))
                .ThrowsAsync(new KeyNotFoundException());
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            
            var result = await _controller.UpdateVote(voteId, voteValue);
            
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
        
        
        [Fact]
        public async Task DeleteVote_ShouldReturnNotFound_WhenVoteDoesNotExist()
        {
            var voteId = Guid.NewGuid();
            var userId = "test_user_id";

            _voteServiceMock.Setup(v => v.Delete(voteId, userId))
                .ThrowsAsync(new KeyNotFoundException());
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            
            var result = await _controller.DeleteVote(voteId);
            
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
        
        
        [Fact]
        public async Task CountPositiveVotesByIdeaId_ShouldReturnZero_WhenNoPositiveVotesExist()
        {
            var ideaId = Guid.NewGuid();
            _voteServiceMock.Setup(v => v.CountPositiveVotesByIdeaId(ideaId))
                .ReturnsAsync(0);
            
            var result = await _controller.CountPositiveVotesByIdeaId(ideaId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, okResult.Value);
        }


        [Fact]
        public async Task CountNegativeVotesByIdeaId_ShouldReturnZero_WhenNoNegativeVotesExist()
        {
            var ideaId = Guid.NewGuid();
            _voteServiceMock.Setup(v => v.CountNegativeVotesByIdeaId(ideaId))
                .ReturnsAsync(0);
            
            var result = await _controller.CountNegativeVotesByIdeaId(ideaId);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, okResult.Value);
        }
    }
}
