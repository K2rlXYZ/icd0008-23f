namespace WebApp01.Data;

public class Pet
{
    public int Id { get; set; }
    
    public string PetName { get; set; }

    public ICollection<PersonPet> PersonPets { get; set; }
    
    
}