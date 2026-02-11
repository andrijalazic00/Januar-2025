namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class KorisnikController : ControllerBase
{
    public IspitContext Context { get; set; }

    public KorisnikController(IspitContext context)
    {
        Context = context;
    }
    
    [HttpPost("DodajKorisnika")]
    public async Task<ActionResult> DodajKorisnika([FromBody] Korisnik korisnik)
    {
        try
        {
            var kor = await Context.korisnici.AddAsync(korisnik);

            await Context.SaveChangesAsync();

            return Ok($"Dodat je korisnik {korisnik.Ime} {korisnik.Prezime} sa ID-em {korisnik.ID}.");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}
