using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
          var walksDomainModel =  await walkRepository.GetAllAsync();


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
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to Dto
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);

        }
    }
}
