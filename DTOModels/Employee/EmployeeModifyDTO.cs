namespace DTOModels;

public class EmployeeModifyDTO
{
    public string? Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public decimal Salary { get; set; }
    public DateOnly HireDate { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }

    public EmployeeModifyDTO(string? id, string lastName, string firstName, string? patronymic, string userName, string password, string role, decimal salary, DateOnly hireDate, DateOnly birthDate, string phone, string city, string street, string zipCode)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        UserName = userName;
        Password = password;
        Role = role;
        Salary = salary;
        HireDate = hireDate;
        BirthDate = birthDate;
        Phone = phone;
        City = city;
        Street = street;
        ZipCode = zipCode;
    }
}