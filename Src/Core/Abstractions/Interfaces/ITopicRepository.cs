﻿using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces
{
    public interface ITopicRepository:IRepositoryGeneric<Topic>
    {
        Task<IEnumerable<Topic>> GetByTitle(string title);
        Task<IEnumerable<Topic>> GetByUser(string userId);
        Task<int> CountTopics();
    }
}