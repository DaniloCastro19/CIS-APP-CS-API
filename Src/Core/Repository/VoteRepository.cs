using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository
{
    public class VoteRepository : IVoteRepository
    {
        private readonly DataContext _context;

        public VoteRepository(DataContext context)
        {
            _context = context;
        }

        private DbSet<Vote> EntitySet => _context.Set<Vote>();


        public async Task<IEnumerable<Vote>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<Vote> GetByID(Guid id)
        {
            return await EntitySet.FindAsync(id.ToString());
        }

        public async Task<Vote> Insert(Vote entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task Update(Vote entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            
            await Save();
        }

        public async Task<Vote> Delete(Guid id)
        {
            var entity = await GetByID(id);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                await Save();
            }
            return entity;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<int> CountVotes()
        {
            return await EntitySet.CountAsync();
        }

        public async Task<IEnumerable<Vote>> GetVotesByUserId(string userId)
        {
            return await EntitySet.Where(v => v.UsersId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Vote>> GetVotesByIdeaId(string ideaId)
        {
            return await EntitySet.Where(v => v.IdeasId == ideaId).ToListAsync();
        }

        public async Task<int> CountPositiveVotesByIdeaId(string ideaId)
        {
            return await EntitySet.CountAsync(v => v.IdeasId == ideaId && v.IsPositive == true);
        }

        public async Task<int> CountNegativeVotesByIdeaId(string ideaId)
        {
            return await EntitySet.CountAsync(v => v.IdeasId == ideaId && v.IsPositive == false);
        }
    }
}