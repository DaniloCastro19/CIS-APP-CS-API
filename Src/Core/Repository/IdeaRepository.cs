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
            context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetByID(id);
            EntitySet.Remove(entity);
            await Save();
        }

        public async Task<IEnumerable<Idea>> GetByContent(string content)
        {
            return await EntitySet.Where(i => i.Content!.Contains(content)).ToListAsync();
        }

        public async Task<int> CountIdeas()
        {
            return await EntitySet.CountAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}