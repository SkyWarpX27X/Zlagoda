namespace DTOModels;

public class EmployeeDTO
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public decimal Salary { get; set; }
    public DateOnly HireDate { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public EmployeeDTO(long id, string fullName, string role, decimal salary, DateOnly birthDate, DateOnly hireDate, string phone, string address)
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