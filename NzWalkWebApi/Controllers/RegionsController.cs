using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalkWebApi.Data;
using NzWalkWebApi.Models.Domain;
using NzWalkWebApi.Models.DTO;
using NzWalkWebApi.Repositories;

namespace NzWalkWebApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        // GET SLL REGION ==========================================>
        // GET : // http://localhost:portNumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();
            //Map Domain Model to  DTo
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl

                });
            }
            //Return DTos
            return Ok(regionsDto);
        }

        //Get Region by Id ==========================================>
        // GET : // http://localhost:portNumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain Model  to Region Dto
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl

            };
            return Ok(regionDto);
        }

        //POST=========================================================>
        [HttpPost]
        public async  Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map Dto to Domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // USe Domain Model Create Region
           await dbContext.Regions.AddAsync(regionDomainModel);
           await dbContext.SaveChangesAsync();


            //Map Domain Model to create Region
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
        }


        //PUT============================================================>
        // PUT : // http://localhost:portNumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           // check if region exist
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);  
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Map Dto to Doamin Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
           await dbContext.SaveChangesAsync();

            // Convert Domain Model to Dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return Ok(regionDto);
        }

        //Delete==============================================================>
        // DELETE : // http://localhost:portNumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> DELETE([FromRoute] Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);   
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Delete region
           dbContext.Regions.Remove(regionDomainModel);
           await dbContext.SaveChangesAsync();

            //return 
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return Ok(regionDto);
        }
        
    }

}
