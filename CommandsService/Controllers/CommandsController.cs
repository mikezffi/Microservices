using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>>GetCommandsForPlatform(int platformId)
        {
            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItems = _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto>GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting command {commandId} for platform: {platformId}");

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(platformId, commandId);
            return Ok(_mapper.Map<CommandReadDto>(command));
        }
        [HttpPost]
        public ActionResult<CommandReadDto>CreateCommandForPlatform(int platformId, CommandCreateDto command)
        {
            Console.WriteLine($"--> Creating command {command}");

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItem = _mapper.Map<Command>(command);
            _repository.CreateCommand(platformId, commandItem);
            _repository.SaveChanges();

            var commandRead = _mapper.Map<CommandReadDto>(commandItem);

            return CreatedAtRoute(
                nameof(GetCommandForPlatform),
                new {platformId = platformId, commandId = commandRead.Id},
                commandRead
            );
        }
    }
}