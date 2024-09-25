using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController:ControllerBase
    {
         private readonly DataContext _dataContext;

        public TopicController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicModel>>> GetTopics()
        {
            return await _dataContext.Topic.ToListAsync();
        }
    }

}

