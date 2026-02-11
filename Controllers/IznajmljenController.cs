namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class IznajmljenController : ControllerBase
{
    public IspitContext Context { get; set; }

    public IznajmljenController(IspitContext context)
    {
        Context = context;
    }
    
    [HttpPost("IznajmiAutomobil/{IDa}/{IDk}/{BrD}")]
    public async Task<ActionResult> IznajmiAutomobil(int IDa, int IDk,uint BrD)
    {
        try
        {
            var iznamljeno = await Context.iznajmljeni.Where(p => p.IznajmljenAutomobil!.ID == IDa).FirstOrDefaultAsync();
            if( iznamljeno != null) return BadRequest("Ovaj automobil je iznajmljen!");
            var auto = await Context.automobili.Where(p => p.ID == IDa).FirstOrDefaultAsync();
            var kor = await Context.korisnici.Where(p => p.ID == IDk).FirstOrDefaultAsync();

            var iznajmi = new Iznajmljen()
            {
                IznajmljenAutomobil = auto,
                KorisnikIznajmljuje = kor,
                DanIznajmljen = BrD
            };

            await Context.iznajmljeni.AddAsync(iznajmi);
            await Context.SaveChangesAsync();
            return Ok($"Korisnik {iznajmi.KorisnikIznajmljuje!.Ime} {iznajmi.KorisnikIznajmljuje!.Prezime} je iznajmio {auto!.Model} na { iznajmi.DanIznajmljen} dana");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}
