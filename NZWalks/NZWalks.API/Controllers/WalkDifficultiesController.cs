using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        //GET=========================================================================================>
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
          var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();
;
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }

        //GETById=========================================================================================>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAllWalkDifficultiesByIdAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetByIdAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);

        }


        //Post==================================================================================>

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            //Call repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert Domain to  Dto
            var WalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetAllWalkDifficultiesByIdAsync), new {id = WalkDifficultyDTO.Id}, WalkDifficultyDTO);
        }


        //Put=============================================================================================================>
        [HttpPut]
        [Route("{id:Guid}")]

        public async  Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to Domain  Models
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //Call repository to update
            walkDifficultyDomain =  await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert Domain To DTO
            var WalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return response
            return Ok(WalkDifficultyDTO);

        }

        //Delete===========================================================================================================>
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //find the id
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if(walkDifficultyDomain == null)
            {
                return NotFound();  
            }

            // Convert To DTO
            var WalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(WalkDifficultyDTO);
        }
    }
}
