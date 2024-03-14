using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositrories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles ="Reader")]

        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            mapper.Map<List<RegionDto>>(regions);
            logger.LogInformation("get all regions action method was invoked");

            logger.LogInformation($"finished get all regions request with data: {JsonSerializer.Serialize(regions)}");

            return Ok(mapper.Map<List<RegionDto>>(regions));

        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
         

            return Ok(mapper.Map<RegionDto>(regionDomain));

        }
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);
            await regionRepository.CreateAsync(regionDomain);
            return Created("region created", addRegionRequestDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            var regionDomain = mapper.Map<Region>(updateRegionRequestDto) ;
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);
            if (regionDomain == null) { return NotFound(); }

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain =await regionRepository.DeleteAsync(id);
            if (regionDomain == null) { return NotFound(); }
            return Ok();
        }


    }


}
