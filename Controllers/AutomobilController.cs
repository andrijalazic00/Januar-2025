using System.Collections.Immutable;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

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
                iznamjmljen = Context.iznajmljeni!.Where(p => p.IznajmljenAutomobil!.ID == s.ID).Select( s => new
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
            if( Pkm == null && BrS== null && CenaV == null && ModelV == null)
            {
               var autos = await Context.automobili.Select(s => new
            {
                s.ID,
                s.Model,
                s.Kilometraza,
                s.Sedista,
                s.CenaPoDanu,
                Iznajmljen = Context.iznajmljeni.Where( p => p.IznajmljenAutomobil!.ID == s.ID).IsNullOrEmpty()? "nije iznajmljen":"iznajmljen"
            }).ToListAsync();
              if( autos == null) return BadRequest("Prodavnica nema automobile");
              return Ok(autos);
            }
            else
            {  
            var autos = new List<Automobil>();
            if( Pkm != null) autos = await Context.automobili.Where(p => p.Kilometraza == Pkm).ToListAsync();
            if(BrS != null)
            {
            if( !autos.IsNullOrEmpty() )
              autos = autos.Where(p => p.Sedista == BrS).ToList();
              else
              autos = await Context.automobili.Where(p => p.Sedista == BrS).ToListAsync();
            }
            if(CenaV != null)
            {
            if(!autos.IsNullOrEmpty())
             autos = autos.Where(p => p.CenaPoDanu == CenaV).ToList(); 
             else
             autos = await Context.automobili.Where(p => p.CenaPoDanu == CenaV).ToListAsync();
            }
            if(ModelV != null)
            {
            if(!autos.IsNullOrEmpty())
            autos = autos.Where(p => p.Model == ModelV).ToList();
            else
            autos = await Context.automobili.Where(p => p.Model == ModelV).ToListAsync();
            }
            if (autos.IsNullOrEmpty()) return BadRequest("Automobil ne postoji");
            var auto = autos.Select( s => new
            {
                s.Model,
                s.Kilometraza,
                s.Sedista,
                s.Godiste,
                s.CenaPoDanu,
                iznajmljen =  Context.iznajmljeni.Where( p => p.IznajmljenAutomobil!.ID == s.ID).IsNullOrEmpty()? "nije iznajmljen":"iznajmljen"
            }).ToList();
            return Ok(auto);
            }
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}
