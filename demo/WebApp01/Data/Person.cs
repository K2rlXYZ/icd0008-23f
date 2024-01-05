namespace WebApp01.Data;

public class Person
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<PersonPet>? PersonPets { get; set; }
    
    
}