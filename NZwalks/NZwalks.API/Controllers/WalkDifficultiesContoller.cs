using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesContoller : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesContoller(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {

            var walkDifficultiesDTO = await walkDifficultyRepository.GetAllAsync();
            mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDTO);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            // convert Domain to DTOs
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficulty([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // convert DTO to domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass domain object to Repository to persist this
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            // Convert the Domain object back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code
            };

            // send DTO response back to client
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, 
            Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // call repository to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // return response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // convert to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }
    }
}
