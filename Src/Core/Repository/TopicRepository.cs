using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly Sd3Context _sd3Context;

        public TopicRepository(Sd3Context sd3Context)
        {
            _sd3Context = sd3Context;
        }

        private DbSet<Topic> EntitySet => _sd3Context.Set<Topic>();

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

<<<<<<< HEAD
        public async Task<Topic> GetByID(string id)
=======
        public async Task<Topic?> GetByID(Guid id)
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
        {
            return await EntitySet.FindAsync(id.ToString());
        }

        public async Task<Topic> Insert(Topic entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task Update(Topic entity)
        {
            _sd3Context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task<Topic> Delete(string id)
        {
            var entity = await GetByID(id);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                await Save();
            }
            return entity;
        }

        public async Task<IEnumerable<Topic>> GetByTitle(string title)
        {
            return await EntitySet.Where(t => t.Title.Contains(title)).ToListAsync();
        }

        public async Task<int> CountTopics()
        {
            return await EntitySet.CountAsync();
        }

        public async Task Save()
        {
            await _sd3Context.SaveChangesAsync();
        }
    }
}