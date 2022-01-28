using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
  public class PlatformDataClient : IPlatformDataClient
  {
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
      _configuration = configuration;
      _mapper = mapper;
    }
    IEnumerable<Platform> IPlatformDataClient.ReturnAllPlatforms()
    {
      Console.WriteLine($"--> Calling Grpc Service {_configuration["GrpcPlatform"]}");
      var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
      var client = new GrpcPlatform.GrpcPlatformClient(channel);
      var request = new GetAllRequest();

      try
      {
        var reply = client.GetAllPlatforms(request);
        return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
      }
      catch (System.Exception ex)
      {
        Console.WriteLine($"--> Could not call Grpc Server {ex.Message}");
        return null;
      }
    }
  }
}