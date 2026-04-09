namespace DTOModels;

public class EmployeeDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public decimal Salary { get; set; }
    public DateOnly HireDate { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public EmployeeDTO(string id, string fullName, string role, decimal salary, DateOnly hireDate, DateOnly birthDate, string phone, string address)
    {
        Id = id;
        FullName = fullName;
        Role = role;
        Salary = salary;
        HireDate = hireDate;
        BirthDate = birthDate;
        Phone = phone;
        Address = address;
    }
}