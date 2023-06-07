using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using FluentValidation;
namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        //GetAll===================================================================================>
        [HttpGet]
        public async Task<IActionResult> GetallRegionsAsync()
        {

            var regions = await regionRepository.GetAllAsync();

            //return DTO
            //var regionsDTO = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(region =>
            //{ 
            //    var regionDTO  = new Models.DTO.Region()
            //    {
            //        Id  = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,


            //    };
            //    regionsDTO.Add(regionDTO);

            //});

            //return DTO
            var regionDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionDTO);

        }

        //GetByID===================================================================================>
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<ActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }


        //Post===================================================================================>
        [HttpPost]

        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate the Rwquest

            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population

            };

            // Pass details To Repository
            region = await regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = region.Id }, regionDTO);
        }



        //GetDelete===================================================================================>
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get the region from database
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            // return 
            return Ok(regionDTO);
        }




        //Update===================================================================================>

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            // validate
            //if (!ValidateAddRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);

            //}

            //Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population

            };
            //Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);


            //If Null then NotFound
            if (region == null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //Return Ok response
            return Ok(regionDTO);
        }




        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                   $"Add Region Data is required.");
                return false;

            }
            if (string.IsNullOrEmpty(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");
            };

            if (string.IsNullOrEmpty(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");
            };

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be les than or equal to zero.");
            };


            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} cannot be les than or equal to zero.");
            };

            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} cannot be les than or equal to zero.");
            };

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cannot be  zero");
            };
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }



        /// /=======================================================================>

        private bool ValidateAddRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                   $"Add Region Data is required.");
                return false;

            }
            if (string.IsNullOrEmpty(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space.");
            };

            if (string.IsNullOrEmpty(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space.");
            };

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be les than or equal to zero.");
            };


            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat),
                    $"{nameof(updateRegionRequest.Lat)} cannot be les than or equal to zero.");
            };

            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                    $"{nameof(updateRegionRequest.Long)} cannot be les than or equal to zero.");
            };

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} cannot be  zero");
            };
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
