using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface ITopicService: IService<Topic, TopicDTO>
{
    Task<IEnumerable<Topic>> GetByTitle(string title);
    Task<IEnumerable<Topic>> GetByUser(Guid userId);
    Task<int> CountTopics();
    Task ValidateOwnership(Guid topicId, string userId);
}
