using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NzWalkWebApi.CustomActionFilters;
using NzWalkWebApi.Models.Domain;
using NzWalkWebApi.Models.DTO;
using NzWalkWebApi.Repositories;

namespace NzWalkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
         private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {

            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        //POST==================================================================================>
        [HttpPost]
        [ValidateModel]
        public async  Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
      
               //Map DTO to Dommain Model
         var walkDomainModel  = mapper.Map<Walk>(addWalksRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain Model to DTO
           var walkDto =  mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);

            }
           

        //GET====================================================================================>
       //GET: /api/walks?filterOn=Name&filterQuery=Track&sort=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery , [FromQuery] string? sortBy, 
            [FromQuery] bool? isAscending, [FromQuery] int pageNumber =1 , [FromQuery] int pageSize =1000 )
        {
          var walksDomainModel =  await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);


              // Map Domain Model to Dto
            var walkDto = mapper.Map<List<WalkDto>>(walksDomainModel);

            return Ok(walkDto);
          
        }

        //GETById=========================================================================>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to Dto
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

         //PUT============================================================================>
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
    {
        // Map DTO to Domain Model
        var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
        if (walkDomainModel == null)
        {
            return NotFound();
        }

        // Map Domain Model to Dto
        var walkDto = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walkDto);

    }

        //DELETE==========================================================================>
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deleteWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model To Dto
            var deleteDto = mapper.Map<WalkDto>(deleteWalkDomainModel);
            return Ok(deleteDto);   
;        }
    }
}
