using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class IdeaDTOBuilder
{
        public IdeaDTOResponse Build(Idea entity)
    {
            return new IdeaDTOResponse{
                Id =entity.Id,
                Title=entity.Title,
                Content=entity.Content,
                CreationDate = entity.CreationDate,
                OwnerID = entity.UsersId,
                OwnerLogin = entity.OwnerLogin,
                TopicID = entity.TopicsId,
                TopicName = entity.TopicName,
            };
    }
}
