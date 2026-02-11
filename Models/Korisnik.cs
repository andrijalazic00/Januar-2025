namespace WebTemplate.Models;

public class Korisnik
{
    [Key]
    public int ID { get; set; }
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    [Length(13,13)]
    public required string JMBG { get; set; }
    [Length(9,9)]
    public required string BrojVozacke { get; set; }
    public List<Iznajmljen>? IznajmljeniAuto { get; set; }
}