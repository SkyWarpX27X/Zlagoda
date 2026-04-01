namespace DBModels;

public class CustomerCardDBModel
{
    public string Number { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public int Percent { get; set; }

    public CustomerCardDBModel(string number, string surname, string name, string? patronymic, string phoneNumber,
        string? city, string? street, string? zipCode, int percent)
    {
        Number = number;
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        PhoneNumber = phoneNumber;
        City = city;
        Street = street;
        ZipCode = zipCode;
        Percent = percent;
    }
}