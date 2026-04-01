namespace DBModels;

public class EmployeeDBModel
{
    public long Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string Role { get; set; }
    public decimal Salary { get; set; }
    public string DateOfBirth { get; set; }
    public string DateOfStart { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string Username { get; set; }
    // No hashing yet!
    public string Password { get; set; }
    
    public EmployeeDBModel(long id, string surname, string name, string? patronymic, string role, decimal salary, string dateOfBirth, 
        string dateOfStart,  string phoneNumber, string city, string street, string zipCode, string username, string password)
    {
        Id = id;
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Role = role;
        Salary = salary;
        DateOfBirth = dateOfBirth;
        DateOfStart = dateOfStart;
        PhoneNumber = phoneNumber;
        City = city;
        Street = street;
        ZipCode = zipCode;
        Username = username;
        Password = password;
    }

    public EmployeeDBModel(string surname, string name, string? patronymic, string role, decimal salary, string dateOfBirth,
        string dateOfStart, string phoneNumber, string city, string street, string zipCode, string username, string password):
        this(0, surname, name, patronymic, role, salary, dateOfBirth, dateOfStart, phoneNumber, city, street, zipCode,
            username, password) {}
}