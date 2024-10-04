using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DataContext _context;

        public TopicRepository(DataContext context)
        {
            _context = context;
        }

        private DbSet<Topic> EntitySet => _context.Set<Topic>();

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<Topic> GetByID(Guid id)
        {
            string userId = id.ToString();
            return await EntitySet.FindAsync(userId);
        }

        public async Task<Topic> Insert(Topic entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task Update(Topic entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetByID(id);
            if (entity != null)
            {
                EntitySet.Remove(entity);
                await Save();
            }
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
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Topic>> GetByUser(string userId)
        {
            return await EntitySet.Where(topic => topic.UsersId == userId).ToListAsync();
        }
    }
}