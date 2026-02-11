namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public DbSet<Automobil> automobili {get; set;}
    public DbSet<Korisnik> korisnici { get; set; }
    public DbSet<Iznajmljen> iznajmljeni { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
