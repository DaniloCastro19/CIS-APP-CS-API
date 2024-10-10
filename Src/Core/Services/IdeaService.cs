using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class IdeaService: IIdeaService
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly OwnershipValidator<Idea> _ownershipValidator;
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;




        public IdeaService(IIdeaRepository ideaRepository,  OwnershipValidator<Idea> ownershipValidator, ITopicService topicService,IUserService userService)
        {
            _ideaRepository = ideaRepository;
            _ownershipValidator = ownershipValidator;
            _topicService = topicService;
            _userService = userService;

        }

        public async Task<IEnumerable<Idea>> GetAll()
        {
            return await _ideaRepository.GetAll();
        }

        public async Task<Idea?> GetByID(Guid id)
        {
            return await _ideaRepository.GetByID(id);
        }

        public async Task Create(Idea idea)
        {
            
            await _ideaRepository.Insert(idea);
        }

        public async Task<IEnumerable<Idea>> GetByUser(Guid userId)
        {
            string id = userId.ToString();
            return await _ideaRepository.GetByUser(id);
        }

        public async Task<int> CountUserIdeas(Guid userId)
        {
            string id = userId.ToString();
            return  await _ideaRepository.CountIdeas(id);
        }

        public async Task<Idea> Create(IdeaDTO entity, string userID, Guid topicID)
        {
            User user = await _userService.GetUserById(userID);
            Topic topic = await _topicService.GetByID(topicID);
            if(topic==null) return null;
            
            var topicIdToString = topicID.ToString();
            Idea newIdea = new Idea
            {
                Id = Guid.NewGuid().ToString(),
                Title= entity.Title,
                Content = entity.Content,
                CreationDate = DateTime.UtcNow,
                UsersId = userID,
                TopicsId = topicIdToString,
                OwnerLogin = user.Login,
                TopicName = topic.Title
            };
            return await _ideaRepository.Insert(newIdea);;
        }

        public async Task<Idea> Update(Guid ideaID, IdeaDTO entity, string userId)
        {
            await _ownershipValidator.ValidateOwnership(ideaID, userId, _ideaRepository);
            var existingIdea = await _ideaRepository.GetByID(ideaID); 
            if (existingIdea == null) return null;
            existingIdea.Title = entity.Title;
            existingIdea.Content = entity.Content;
            await _ideaRepository.Update(existingIdea);
            return existingIdea;
        }
        
        public async Task Delete(Guid id, string userId)
        {
            await _ownershipValidator.ValidateOwnership(id, userId, _ideaRepository);
            await _ideaRepository.Delete(id);
        }
    }
}