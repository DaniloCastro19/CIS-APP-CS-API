using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository
{
    public class IdeaRepository: IIdeaRepository
    {
        private DbSet<Idea> EntitySet => context.Set<Idea>();
        private readonly IVoteRepository _voteRepository;
        DataContext context;
        public IdeaRepository(DataContext Context, IVoteRepository voteRepository) {
            context = Context;
            _voteRepository = voteRepository;
        }
    
        public async Task<IEnumerable<Idea>> GetAll(bool mostWanted)
        {
            var ideas = await EntitySet.ToListAsync();

            if (mostWanted)
            {
                return ideas.OrderByDescending(
                    idea => _voteRepository.CountPositiveVotesByIdeaId(idea.Id).Result
                ).ToList();
            }
            return ideas;
        }

        public async Task<Idea?> GetByID(Guid id) 
        {
            return await EntitySet.FindAsync(id.ToString()); 
        }

        public async Task<Idea> Insert(Idea entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task Update(Idea entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetByID(id);
            EntitySet.Remove(entity);
            await Save();
        }

        public async Task<int> CountIdeas(string id)
        {
            return await EntitySet.Where(idea => idea.UsersId == id).CountAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Idea>> GetByUser(string userId)
        {
            return await EntitySet.Where(idea => idea.UsersId == userId).ToListAsync();

        }

        public async Task<IEnumerable<Idea>> GetAll()
        {
            return await EntitySet.ToListAsync();

        }
    }
}