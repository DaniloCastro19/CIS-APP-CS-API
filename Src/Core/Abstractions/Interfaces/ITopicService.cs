using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface ITopicService
{
    Task<IEnumerable<Topic>> GetAll();
    Task<Topic> GetByID(Guid id);
    Task<Topic> Create(TopicDTO entity, string userID);
    Task Update(TopicDTO entity, string userID, Guid topicID);
    Task Delete(Guid id, string userID);
    Task<IEnumerable<Topic>> GetByTitle(string title);
    Task<IEnumerable<Topic>> GetByUser(Guid userId);
    Task<int> CountTopics();
}
