namespace WebTemplate.Models;

public class Iznajmljen
{
    [Key]
    public int ID { get; set; }
    public uint? DanIznajmljen { get; set; }
    [ForeignKey("IznajmljenAutomobilFK")]
    public Automobil? IznajmljenAutomobil { get; set; }
    [ForeignKey("KorisnikIznajmljujeFK")]
    public Korisnik? KorisnikIznajmljuje { get; set; }
}