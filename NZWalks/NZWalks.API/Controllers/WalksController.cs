using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        //GETAll==================================================================>
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch DATA FROM   DATABASE- DOMAIN WALKS
            var walksDomain = await walkRepository.GetAllAsync();

            //   Convert Domain walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response
            return Ok(walksDTO);
        }
        //GETById==================================================================>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkByIdAsync(Guid id)
        {
            // Get Domain object from database
            var walkDomain = await walkRepository.GetByIdAsync(id);

            //Convert Domain object to Dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //return respone 
            return Ok(walkDTO);
        }

        //Post==================================================================>
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]  Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO into Domain Object 
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                walkDifficultyId = addWalkRequest.walkDifficultyId
            };

            //Pass domain object to Repository to persist this 
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //Convert the Domain object back to Dto
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                walkDifficultyId = walkDomain.walkDifficultyId
            };
            ///Send Dto response back to Client
            return CreatedAtAction(nameof(GetWalkByIdAsync), new { id = walkDTO.Id }, walkDTO);
        }



        //Update==================================================================>

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTo to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                walkDifficultyId = updateWalkRequest.walkDifficultyId

            };
            // Pass Details to Repository - Get Domain object in response(or null)
            walkDomain = await walkRepository.UpdateAsync(id,walkDomain);
            // HAndle Null (Not found)
            if (walkDomain == null)
            {
                return null; 
            }
            // Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                walkDifficultyId = walkDomain.walkDifficultyId
            };
            //Return Response
            return Ok(walkDTO);
        }



        //Delete==================================================================>
        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Call Repository to Delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain); 
            return Ok(walkDTO);
        }
    }
}
