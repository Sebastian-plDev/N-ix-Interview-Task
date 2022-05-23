using Microsoft.EntityFrameworkCore;
using MvcProject.Entities;
public class EFContext : DbContext
{
  public EFContext(DbContextOptions<EFContext> options) : base(options)
  { }

   protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity
        options.UseInMemoryDatabase("TestDb");
    }

  public DbSet<ShortUrl> ShortUrls { get; set; }

}