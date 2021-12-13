using PlatformService.Models;

namespace PlatformService.Data
{
  public static class PrepDb 
  {
    public static void PrepPopulation(IApplicationBuilder app)
    {
      using(var serviceScope = app.ApplicationServices.CreateScope())
      {
        SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
      }
    }

    private static void SeedData(ApplicationDbContext context)
    {
      if (!context.Platforms.Any())
      {
        Console.WriteLine("--> Seeding data...");
        context.Platforms.AddRange(
          new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
          new Platform() {Name="Sql Server Express", Publisher="Microsoft", Cost="Free"},
          new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
        );
        context.SaveChanges();
      }
      else
      {
        Console.WriteLine("--> We already have data.");
      }
    }
  }
}