namespace WebApp01.Data;

public class PersonPet
{
    public int Id { get; set; }

    
    public int PercentageOwned { get; set; } = 100;

    public int PersonId { get; set; }
    public Person Person { get; set; }

    public int PetId { get; set; }
    public Pet Pet { get; set; }
    
    
}