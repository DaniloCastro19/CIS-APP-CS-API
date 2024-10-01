using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces
{
    public interface IIdeaRepository:IRepositoryGeneric<Idea>
    {
        Task<IEnumerable<Idea>> GetByContent(string content);
        Task<int> CountIdeas();
    }
}