namespace DBModels;

public class EmployeeContactInfoDataModel(string surname, string phoneNumber, string city, string street, string zipCode)
{
    public string Surname { get; set; } = surname;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string City { get; set; } = city;
    public string Street { get; set; } = street;
    public string ZipCode { get; set; } = zipCode;
}