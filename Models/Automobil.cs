namespace WebTemplate.Models;

public class Automobil
{   
    [Key]
    public int ID { get; set; }
    public required string Model { get; set; }
    public uint Kilometraza { get; set; }
    public uint  Godiste { get; set; }
    public uint Sedista { get; set; }
    public uint CenaPoDanu { get; set; }
    
    public Iznajmljen? IznajmljitiKorisniku { get; set; }
}