using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class AutomobilController : ControllerBase
{
    public IspitContext Context { get; set; }

    public AutomobilController(IspitContext context)
    {
        Context = context;
    }
    
    [HttpPost("DodajAutomobil")]
    public async Task<ActionResult> DodajAutomobil([FromBody] Automobil automobil)
    {
        try
        {
           var auto = await Context.automobili.AddAsync(automobil);

           await Context.SaveChangesAsync();

            return Ok($"Dodat je automobil {automobil.Model} sa ID-em {automobil.ID}. ");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("VratiAutomobil/{IDa}")]
    public async Task<ActionResult> VratiAutomobil(int IDa)
    {
        try
        {
            var auto = await Context.automobili.Where(p => p.ID == IDa).FirstOrDefaultAsync();
            if(auto == null) return BadRequest("Kola tog ID-a ne postoje");
            return Ok(auto);
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("VratiAutomobile")]
    public async Task<ActionResult> VratiAutomobile()
    {
        try
        {
            var autos = await Context.automobili.Select(s => new
            {
                s.ID,
                s.Model,
                s.Kilometraza,
                s.Sedista,
                s.CenaPoDanu,
                i = Context.iznajmljeni!.Where(p => p.IznajmljenAutomobil!.ID == s.ID).Select( s => new
                {
                    s.ID,
                    i = Context.korisnici!.Where(k => k.ID == s.KorisnikIznajmljuje!.ID).Select( s => new
                    {
                        s.ID,
                        s.Ime,
                        s.Prezime,
                        s.BrojVozacke
                    }).FirstOrDefault()
                }).FirstOrDefault()
                }).ToListAsync();
            if( autos == null) return BadRequest("Lista je prazna!");

            return Ok(autos);
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("FiltrirajAutomobile")]
    public async Task<ActionResult> FiltrirajAutomobile(uint? Pkm,uint? BrS,uint? CenaV,string? ModelV)
    {
        try
        {
            var autos = await Context.automobili.Where(p => p.Kilometraza == Pkm || p.Sedista == BrS || p.CenaPoDanu == CenaV || p.Model == ModelV).ToListAsync();
            if(autos == null) return BadRequest("Nema takvog automobila!");

            return Ok(autos);
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}
