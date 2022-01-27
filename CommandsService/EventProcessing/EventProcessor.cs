using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
  public class EventProcessor : IEventProcessor
  {
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
      _mapper = mapper;
      _scopeFactory = scopeFactory;
    }

    public void ProcessEvent(string message)
    {
      var eventType = DetermineEvent(message);

      switch (eventType)
      {
        case EventType.PlatformPublished:
          AddPlatform(message);
          break;  
        default:
          break;
      }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published event detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Platform Published event detected");
                return EventType.Undetermined;
        }
    }
    private void AddPlatform(string platformPublishedMessage)
    {
      using (var scope = _scopeFactory.CreateScope())
      {
        var repository = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
          var plat = _mapper.Map<Platform>(platformPublishedDto);
          if(!repository.ExternalPlatformExists(plat.ExternalId))
          {
            repository.CreatePlatform(plat);
            repository.SaveChanges();
          }
          else
          {
            Console.WriteLine("--> Platform already exists...");
          }
        }
        catch (System.Exception ex)
        {
          Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
        }
      }
    }
    
  }

  //ENUM
  enum EventType
  {
    PlatformPublished,
    Undetermined
  }
}