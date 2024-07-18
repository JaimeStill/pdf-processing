namespace Pdf.Models;
public class Person
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Ssn { get; set; }
    public string Gender { get; set; }
    public string Race { get; set; }
    public string CityOfBirth { get; set; }
    public string StateOfBirth { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public bool Citizen { get; set; }

    public static Person Generate() => new()
    {
        FirstName = "John",
        MiddleName = "Allen",
        LastName = "Doe",
        Ssn = "123456789",
        Gender = "Male",
        Race = "White",
        CityOfBirth = "Cleburne",
        StateOfBirth = "TX",
        DateOfBirth = new DateOnly(1985, 11, 5),
        Citizen = true
    };
}