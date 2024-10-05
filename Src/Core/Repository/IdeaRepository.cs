using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository
{
    public class IdeaRepository(DataContext context) : IIdeaRepository
    {
        private DbSet<Idea> EntitySet => context.Set<Idea>();

        public async Task<IEnumerable<Idea>> GetAll()
        {
            return await EntitySet.ToListAsync();
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
    }
}