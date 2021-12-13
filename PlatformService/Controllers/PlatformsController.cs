using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(IPlatformRepository repository, IMapper autoMapper, ICommandDataClient commandDataClient)
    {
      _repository = repository;
      _mapper = autoMapper;
      _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      Console.WriteLine("-> Getting Platforms...");

      var platformItems = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
      Console.WriteLine("-> Getting platform by id...");

      var platformItem = _repository.GetPlatformById(id);

      if (platformItem != null)
      {
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
      }

      return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformCreateDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
      var platformModel = _mapper.Map<Platform>(platformCreateDto);

      _repository.CreatePlatform(platformModel);
      _repository.SaveChanges();

      var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

      try
      {
        await _commandDataClient.SendPlatformToCommand(platformReadDto);
      }
      catch (System.Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }
  }
} 