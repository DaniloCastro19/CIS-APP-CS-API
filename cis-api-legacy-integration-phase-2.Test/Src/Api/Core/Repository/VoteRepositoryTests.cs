using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace cis_api_legacy_integration_phase_2.Test.Api.Core.Repository
{
    public class VoteRepositoryTests
    {
        private readonly DataContext _context;
        private readonly VoteRepository _repository;

        public VoteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _repository = new VoteRepository(_context);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllVotes()
        {
            var votes = new List<Vote>
            {
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea2", IdeaName = "Name 2",  UsersId = "user2",OwnerLogin = "user2", IsPositive = false }
            };
            await _context.Set<Vote>().AddRangeAsync(votes);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAll();

            Assert.Equal(2, result.Count());
            Assert.Equal(votes.Select(v => v.IdeasId), result.Select(v => v.IdeasId));
        }

        [Fact]
        public async Task GetByID_ShouldReturnCorrectVote()
        {
            var voteId = Guid.NewGuid();
            var vote = new Vote { Id = voteId.ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1",OwnerLogin = "user1", IsPositive = true };
            await _context.Set<Vote>().AddAsync(vote);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByID(voteId);

            Assert.NotNull(result);
            Assert.Equal(vote.Id, result.Id);
        }

        [Fact]
        public async Task Insert_ShouldAddNewVote()
        {
            var newVote = new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true };

            var result = await _repository.Insert(newVote);

            Assert.NotNull(result);
            Assert.Equal(newVote.Id, result.Id);
            Assert.Equal(1, await _context.Set<Vote>().CountAsync());
        }

        [Fact]
        public async Task Update_ShouldModifyExistingVote()
        {
            var vote = new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true };
            await _context.Set<Vote>().AddAsync(vote);
            await _context.SaveChangesAsync();

            vote.IsPositive = false;
            await _repository.Update(vote);

            var updatedVote = await _context.Set<Vote>().FindAsync(vote.Id);
            Assert.NotNull(updatedVote);
            Assert.False(updatedVote.IsPositive);
        }

        [Fact]
        public async Task Delete_ShouldRemoveVote()
        {
            var voteId = Guid.NewGuid();
            var vote = new Vote { Id = voteId.ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true };
            await _context.Set<Vote>().AddAsync(vote);
            await _context.SaveChangesAsync();

            await _repository.Delete(voteId);

            var deletedVote = await _context.Set<Vote>().FindAsync(voteId.ToString());
            Assert.Null(deletedVote);
        }

        [Fact]
        public async Task GetVotesByUserId_ShouldReturnUserVotes()
        {
            var userId = "user1";
            var votes = new List<Vote>
            {
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = userId, OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea2", IdeaName = "Name 2", UsersId = userId, OwnerLogin = "user1", IsPositive = false },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea3", IdeaName = "Name 3", UsersId = "other_user", OwnerLogin = "user1", IsPositive = true }
            };
            await _context.Set<Vote>().AddRangeAsync(votes);
            await _context.SaveChangesAsync();

            var result = await _repository.GetVotesByUserId(userId);

            Assert.Equal(2, result.Count());
            Assert.All(result, vote => Assert.Equal(userId, vote.UsersId));
        }

        [Fact]
        public async Task GetVotesByIdeaId_ShouldReturnIdeaVotes()
        {
            var ideaId = "idea1";
            var votes = new List<Vote>
            {
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user2", OwnerLogin = "user1", IsPositive = false },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea2", IdeaName = "Name 2",  UsersId = "user3", OwnerLogin = "user1", IsPositive = true }
            };
            await _context.Set<Vote>().AddRangeAsync(votes);
            await _context.SaveChangesAsync();

            var result = await _repository.GetVotesByIdeaId(ideaId);

            Assert.Equal(2, result.Count());
            Assert.All(result, vote => Assert.Equal(ideaId, vote.IdeasId));
        }

        [Fact]
        public async Task CountPositiveVotesByIdeaId_ShouldReturnCorrectCount()
        {
            var ideaId = "idea1";
            var votes = new List<Vote>
            {
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user2", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea2", IdeaName = "Name 2", UsersId = "user3", OwnerLogin = "user1", IsPositive = false }
            };
            await _context.Set<Vote>().AddRangeAsync(votes);
            await _context.SaveChangesAsync();

            var count = await _repository.CountPositiveVotesByIdeaId(ideaId);

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task CountNegativeVotesByIdeaId_ShouldReturnCorrectCount()
        {
            var ideaId = "idea1";
            var votes = new List<Vote>
            {
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = false },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user2", OwnerLogin = "user1", IsPositive = true },
                new Vote { Id = Guid.NewGuid().ToString(), IdeasId = ideaId, IdeaName = "Name 1", UsersId = "user3", OwnerLogin = "user1", IsPositive = false }
            };
            await _context.Set<Vote>().AddRangeAsync(votes);
            await _context.SaveChangesAsync();

            var count = await _repository.CountNegativeVotesByIdeaId(ideaId);

            Assert.Equal(2, count);
        }
        
        //Negative tests
        [Fact]
        public async Task GetByID_ShouldReturnNull_WhenVoteDoesNotExist()
        {
            var nonExistentVoteId = Guid.NewGuid();
            var result = await _repository.GetByID(nonExistentVoteId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ShouldThrowException_WhenVoteDoesNotExist()
        {
            var nonExistentVote = new Vote { Id = Guid.NewGuid().ToString(), IdeasId = "idea1", IdeaName = "Name 1", UsersId = "user1", OwnerLogin = "user1", IsPositive = true };
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _repository.Update(nonExistentVote));
        }

        [Fact]
        public async Task Delete_ShouldNotThrowException_WhenVoteDoesNotExist()
        {
            var nonExistentVoteId = Guid.NewGuid();
            var exception = await Record.ExceptionAsync(async () => await _repository.Delete(nonExistentVoteId));
            Assert.Null(exception);
            var count = await _context.Set<Vote>().CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetVotesByUserId_ShouldReturnEmpty_WhenUserHasNoVotes()
        {
            var userId = "non_existing_user";
            var result = await _repository.GetVotesByUserId(userId);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetVotesByIdeaId_ShouldReturnEmpty_WhenIdeaHasNoVotes()
        {
            var ideaId = "non_existing_idea";
            var result = await _repository.GetVotesByIdeaId(ideaId);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CountPositiveVotesByIdeaId_ShouldReturnZero_WhenIdeaHasNoVotes()
        {
            var ideaId = "non_existing_idea";
            var count = await _repository.CountPositiveVotesByIdeaId(ideaId);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CountNegativeVotesByIdeaId_ShouldReturnZero_WhenIdeaHasNoVotes()
        {
            var ideaId = "non_existing_idea";
            var count = await _repository.CountNegativeVotesByIdeaId(ideaId);
            Assert.Equal(0, count);
        }
    }
}
