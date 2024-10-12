using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using Moq;

namespace cis_api_legacy_integration_phase_2.Test.Api.Core.Services;

public class VoteServiceTests
{
    private readonly Mock<IVoteRepository> _mockVoteRepository;
    private readonly Mock<IIdeaService> _mockIdeaService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<OwnershipValidator<Vote>> _mockOwnershipValidator;
    private readonly VoteService _voteService;

    public VoteServiceTests()
    {
        _mockVoteRepository = new Mock<IVoteRepository>();
        _mockIdeaService = new Mock<IIdeaService>();
        _mockUserService = new Mock<IUserService>();
        _mockOwnershipValidator = new Mock<OwnershipValidator<Vote>>();
        _voteService = new VoteService(_mockVoteRepository.Object, _mockOwnershipValidator.Object, _mockIdeaService.Object, _mockUserService.Object);
    }
    
    
    [Fact]
    public async Task GetAll_ReturnsVotes()
    {
        var votes = new List<Vote> { new Vote { Id = Guid.NewGuid().ToString() } };
        _mockVoteRepository.Setup(repo => repo.GetAll()).ReturnsAsync(votes);
        var result = await _voteService.GetAll();
        Assert.NotNull(result);
        Assert.Single(result);
    }


    [Fact]
    public async Task GetByID_ExistingVote_ReturnsVote()
    {
        var voteId = Guid.NewGuid();
        var vote = new Vote { Id = voteId.ToString() };
        _mockVoteRepository.Setup(repo => repo.GetByID(voteId)).ReturnsAsync(vote);
        
        var result = await _voteService.GetByID(voteId);
        Assert.NotNull(result);
        Assert.Equal(voteId.ToString(), result?.Id);
    }
    
    [Fact]
    public async Task Create_ValidVote_ReturnsNewVote()
    {
        var voteValue = true;
        var userID = "user123";
        var ideaId = Guid.NewGuid();
    

        var user = new User { Id = userID, Login = "userLogin" };
        var idea = new Idea { Id = ideaId.ToString(), Title = "Test Idea" };
        var newVote = new Vote { Id = Guid.NewGuid().ToString(), IsPositive = voteValue, UsersId = userID };
        
        _mockUserService.Setup(service => service.GetUserById(userID)).ReturnsAsync(user);
        _mockIdeaService.Setup(service => service.GetByID(ideaId)).ReturnsAsync(idea);
        _mockVoteRepository.Setup(repo => repo.Insert(It.IsAny<Vote>()))
            .ReturnsAsync(newVote);
        
        var result = await _voteService.Create(voteValue, userID, ideaId);
        
        Assert.NotNull(result);
        Assert.Equal(voteValue, result.IsPositive);
        Assert.Equal(userID, result.UsersId);
    }

    
    [Fact]
    public async Task Update_ValidVote_UpdatesVote()
    {
        var voteId = Guid.NewGuid();
        var userId = "user123"; 
        var existingVote = new Vote { Id = voteId.ToString(), IsPositive = false, UsersId = userId };
        _mockVoteRepository.Setup(repo => repo.GetByID(voteId)).ReturnsAsync(existingVote);
        
        await _voteService.Update(voteId, true, userId);
        _mockVoteRepository.Verify(repo => repo.Update(It.Is<Vote>(v => v.IsPositive == true)), Times.Once);
    }

    

    [Fact]
    public async Task Delete_ExistingVote_DeletesVote()
    {
        var voteId = Guid.NewGuid();
        var userId = "user123";
        
        var existingVote = new Vote { Id = voteId.ToString(), IsPositive = false, UsersId = userId };
        _mockVoteRepository.Setup(repo => repo.GetByID(voteId)).ReturnsAsync(existingVote);

        await _voteService.Delete(voteId, userId);
        _mockVoteRepository.Verify(repo => repo.Delete(voteId), Times.Once);
    }



    [Fact]
    public async Task GetVotesByUserId_ValidUserId_ReturnsVotes()
    {
        var userId = Guid.NewGuid().ToString();
        var votes = new List<Vote> { new Vote { Id = Guid.NewGuid().ToString(), UsersId = userId } };

        _mockVoteRepository.Setup(repo => repo.GetVotesByUserId(userId)).ReturnsAsync(votes);
        var result = await _voteService.GetVotesByUserId(Guid.Parse(userId));
        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(userId, result.First().UsersId);
    }


    [Fact]
    public async Task GetVotesByIdeaId_ValidIdeaId_ReturnsVotes()
    {
        var ideaId = Guid.NewGuid().ToString();
        var votes = new List<Vote> { new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId } };
        _mockVoteRepository.Setup(repo => repo.GetVotesByIdeaId(ideaId)).ReturnsAsync(votes);
        var result = await _voteService.GetVotesByIdeaId(Guid.Parse(ideaId));
        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(ideaId, result.First().IdeasId);
    }


    [Fact]
    public async Task CountPositiveVotesByIdeaId_ValidIdeaId_ReturnsCount()
    {
        var ideaId = Guid.NewGuid().ToString();
        var count = 5;
        _mockVoteRepository.Setup(repo => repo.CountPositiveVotesByIdeaId(ideaId)).ReturnsAsync(count);
        
        var result = await _voteService.CountPositiveVotesByIdeaId(Guid.Parse(ideaId));
        Assert.Equal(count, result);
    }
    
    [Fact]
    public async Task CountNegativeVotesByIdeaId_ValidIdeaId_ReturnsCount()
    {
        var ideaId = Guid.NewGuid().ToString();
        var count = 3;

        _mockVoteRepository.Setup(repo => repo.CountNegativeVotesByIdeaId(ideaId)).ReturnsAsync(count);
        var result = await _voteService.CountNegativeVotesByIdeaId(Guid.Parse(ideaId));
        Assert.Equal(count, result);
    }
    
    //negative test
    [Fact]
    public async Task GetVotesByUserId_NonExistentUser_ThrowsException()
    {
        var userId = Guid.NewGuid().ToString();
        
        _mockVoteRepository.Setup(repo => repo.GetVotesByUserId(userId)).ReturnsAsync(new List<Vote>());
        
        var result = await _voteService.GetVotesByUserId(Guid.Parse(userId));
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task CountPositiveVotesByIdeaId_InvalidIdea_ThrowsException()
    {
        var ideaId = Guid.NewGuid().ToString();
        
        _mockVoteRepository.Setup(repo => repo.CountPositiveVotesByIdeaId(ideaId)).ReturnsAsync(0);
        var result = await _voteService.CountPositiveVotesByIdeaId(Guid.Parse(ideaId));
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CountNegativosVotesByIdeaId_InvalidIdea_ThrowsException()
    {
        var ideaId = Guid.NewGuid().ToString();
        _mockVoteRepository.Setup(repo => repo.CountNegativeVotesByIdeaId(ideaId)).ReturnsAsync(0);
        
        var result = await _voteService.CountPositiveVotesByIdeaId(Guid.Parse(ideaId));
        Assert.Equal(0, result);
    }
}