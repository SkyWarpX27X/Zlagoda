namespace DBModels;

public class CustomerCardDBModel(string number, string surname, string name, string? patronymic, string phoneNumber,
    string? city, string? street, string? zipCode, int percent)
{
    public string Number { get; set; } = number;
    public string Surname { get; set; } = surname;
    public string Name { get; set; } = name;
    public string? Patronymic { get; set; } = patronymic;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string? City { get; set; } = city;
    public string? Street { get; set; } = street;
    public string? ZipCode { get; set; } = zipCode;
    public int Percent { get; set; } = percent;
}