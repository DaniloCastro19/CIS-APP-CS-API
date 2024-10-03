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
            string ideaId = id.ToString();
            return await EntitySet.FindAsync(ideaId); 
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

        public async Task<Idea?> Delete(Guid id)
        {
            var entity = await GetByID(id);
            if (entity == null) return entity;
            EntitySet.Remove(entity);
            await Save();
            return entity;
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