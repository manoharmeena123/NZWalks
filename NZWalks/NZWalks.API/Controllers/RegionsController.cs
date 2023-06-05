using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper
            )
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
  

     
        [HttpGet]
        public IActionResult GetallRegions()
        {
            
            var  regions  = regionRepository.GetAll();

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
    }
}
