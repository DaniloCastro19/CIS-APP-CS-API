using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface ITopicService: IService<Topic>
{
    Task<IEnumerable<Topic>> GetByTitle(string title);
    Task<int> CountTopics();
}
