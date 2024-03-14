using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositrories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            Mapper = mapper;
            this.walkRepository = walkRepository;
        }

       

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddWalkRequestDto addWalkRequestDto)
        {
            Walk walkDomainModel = Mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            return Ok(Mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn ,
            [FromQuery]string? filterQuery, 
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize=1000)
        {

            List<Walk> walks = await walkRepository.GetAllAsync(filterOn,filterQuery,pageNumber,pageSize);
            throw new Exception("this is a new exception");
            return Ok(Mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walkDomianModel=await walkRepository.GetByIdAsync(id);
            if(walkDomianModel == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<WalkDto>(walkDomianModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel=Mapper.Map<Walk>(updateWalkRequestDto);
            Walk walk  = await walkRepository.UpdateAsync(id, walkDomainModel);
            if(walk == null)
            {
                return NotFound(id);
            }
            return Ok(Mapper. Map<WalkDto>(walkDomainModel));
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            var deletedWalk=await walkRepository.DeleteAsync(id);   
            if (deletedWalk == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<WalkDto>(deletedWalk));
        }
    }
}
