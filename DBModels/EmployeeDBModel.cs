namespace DBModels;

public class EmployeeDBModel(long id, string surname, string name, string? patronymic, string role, decimal salary,
    string dateOfBirth, string dateOfStart, string phoneNumber, string city, string street, string zipCode, 
    string username, string password)
{
    public long Id { get; set; } = id;
    public string Surname { get; set; } = surname;
    public string Name { get; set; } = name;
    public string? Patronymic { get; set; } = patronymic;
    public string Role { get; set; } = role;
    public decimal Salary { get; set; } = salary;
    public string DateOfBirth { get; set; } = dateOfBirth;
    public string DateOfStart { get; set; } = dateOfStart;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string City { get; set; } = city;
    public string Street { get; set; } = street;
    public string ZipCode { get; set; } = zipCode;

    public string Username { get; set; } = username;

    // No hashing yet!
    public string Password { get; set; } = password;

    public EmployeeDBModel(string surname, string name, string? patronymic, string role, decimal salary, string dateOfBirth,
        string dateOfStart, string phoneNumber, string city, string street, string zipCode, string username, string password):
        this(0, surname, name, patronymic, role, salary, dateOfBirth, dateOfStart, phoneNumber, city, street, zipCode,
            username, password) {}
}