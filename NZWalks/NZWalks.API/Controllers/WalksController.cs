using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper , IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository )
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        //GETAll==================================================================>
        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody]  Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Validate 

            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);

            }
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Validation
            if (!(await UpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }   
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
        [Authorize(Roles = "writer")]
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



        #region Private methods
        //PostValidation=================================>
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //       $"Add Walk Data is required.");
            //    return false;

            //}
            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} is required");
            //}
            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} should be greater than zero ");
            //}
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                   $"{nameof(addWalkRequest.RegionId)} is Invalid ");

            }
            var walkDifficulty = await walkDifficultyRepository.GetByIdAsync(addWalkRequest.walkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.walkDifficultyId),
                   $"{nameof(addWalkRequest.walkDifficultyId)} is Invalid ");

            }
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        //Updatevalidation================================>
        private async Task<bool> UpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //       $"Add Walk Data is required.");
            //    return false;

            //}
            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} is required");
            //}
            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} should be greater than zero ");
            //}
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                   $"{nameof(updateWalkRequest.RegionId)} is Invalid ");

            }
            var walkDifficulty = await walkDifficultyRepository.GetByIdAsync(updateWalkRequest.walkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.walkDifficultyId),
                   $"{nameof(updateWalkRequest.walkDifficultyId)} is Invalid ");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
