using PlatformService.Models;

namespace PlatformService.Data
{
  public class PlatformRepository : IPlatformRepository
  {
    private readonly ApplicationDbContext _context;

    public PlatformRepository(ApplicationDbContext context)
    {
      _context = context;
    }
    public void CreatePlatform(Platform model)
    {
      if (model == null)
      {
          throw new ArgumentNullException(nameof(model));
      }
      
      _context.Platforms.Add(model);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
      return _context.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
      return _context.Platforms.FirstOrDefault(platform => platform.Id == id);
    }

    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }
  }
}