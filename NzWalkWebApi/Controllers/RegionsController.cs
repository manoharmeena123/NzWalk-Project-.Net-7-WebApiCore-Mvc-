using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalkWebApi.CustomActionFilters;
using NzWalkWebApi.Data;
using NzWalkWebApi.Models.Domain;
using NzWalkWebApi.Models.DTO;
using NzWalkWebApi.Repositories;
using System.Web.Http.ModelBinding;

namespace NzWalkWebApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
           IMapper mapper )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET ALL REGION ==========================================>
        // GET : // http://localhost:portNumber/api/regions
        [HttpGet]
        [Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();
         
            //Map Domain Model to  DTo
          var regionsDto =  mapper.Map<List<RegionDto>>(regionsDomain);

            //Return DTos
            return Ok(regionsDto);
        }

        //Get Region by Id ==========================================>
        // GET : // http://localhost:portNumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain Model  to Region Dto
         var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        //POST=========================================================>
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async  Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
          
                //Map Dto to Domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                   // USe Domain Model Create Region
              regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model to create Region
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
            }
     
    
    //PUT============================================================>
    // PUT : // http://localhost:portNumber/api/regions/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
     [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {

        //Map DTO to Domain Model
        var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
        // check if region exist
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Convert Domain Model to Dto
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);
        return Ok(regionDto);

        }

        //Delete==============================================================>
        // DELETE : // http://localhost:portNumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async  Task<IActionResult> DELETE([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
       
            //return 
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
        
    }

}
