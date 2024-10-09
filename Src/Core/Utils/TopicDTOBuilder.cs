using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class TopicDTOBuilder
{
    public TopicDTOResponse Build(Topic entity, UserDto user)
    {
            return new TopicDTOResponse{
                Id =entity.Id,
                Title=entity.Title,
                Description=entity.Description,
                CreationDate=entity.CreationDate,
                User= user,
            };
    }

}
