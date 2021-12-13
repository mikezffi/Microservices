using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
  [ApiController]
  [Route("api/c/[controller]")]
  public class PlatformsController : ControllerBase
  {
    public PlatformsController()
    {
        
    }

    [HttpPost]
    public IActionResult TestInboundConnection()
    {
      Console.WriteLine("--> Inbound POST # Command Server");

      return Ok("Inbound test");
    }
  }
}