using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DBModels;
using DTOModels;
using Repositories.Employee;

namespace Services.Employee;

public class EmployeeService : IEmployeeService
{
    private const int HashSize = 32;
    private const int SaltSize = 16;
    
    private IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void AddEmployee(EmployeeModifyDTO employee)
    {
        if (string.IsNullOrEmpty(employee.LastName)) throw new InvalidDataException("Last name cannot be empty");
        if (string.IsNullOrEmpty(employee.FirstName)) throw new InvalidDataException("First name cannot be empty");
        if (string.IsNullOrEmpty(employee.UserName)) throw new InvalidDataException("Username cannot be empty");
        if (string.IsNullOrEmpty(employee.Role)) throw new InvalidDataException("Role cannot be empty");
        if (employee.Salary < 0) throw new InvalidDataException("Salary cannot be negative");
        if (string.IsNullOrEmpty(employee.Phone)) throw new InvalidDataException("Phone number cannot be empty");
        if (!Regex.IsMatch(employee.Phone, @"\+\d{1,12}"))
            throw new InvalidDataException("Invalid phone number");
        int age = DateTime.Now.Year - employee.BirthDate.Year;
        if (employee.BirthDate.AddYears(age).ToDateTime(new(0, 0)) > DateTime.Now) --age;
        if (age < 18) throw new InvalidDataException("Worker can't be younger than 18 years old");
        if (string.IsNullOrEmpty(employee.City)) throw new InvalidDataException("City cannot be empty");
        if (string.IsNullOrEmpty(employee.Street)) throw new InvalidDataException("Street cannot be empty");
        if (string.IsNullOrEmpty(employee.ZipCode)) throw new InvalidDataException("Zip code cannot be empty");
        if (!Regex.IsMatch(employee.Phone, @"\d{5}"))
            throw new InvalidDataException("Invalid zip code");
        
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(salt);
        byte[] passwordHash = HashPassword(employee.Password, salt);
        string password = Convert.ToBase64String(passwordHash);
        
        _employeeRepository.AddEmployee(new EmployeeDBModel(
            employee.LastName,
            employee.FirstName,
            employee.Patronymic,
            employee.Role, employee.Salary,
            employee.BirthDate.ToShortDateString(),
            employee.HireDate.ToShortDateString(),
            employee.Phone,
            employee.City,
            employee.Street,
            employee.ZipCode,
            employee.UserName,
            password));
    }

    public void UpdateEmployee(EmployeeModifyDTO employee)
    {
        if (employee.Id is null) throw new InvalidDataException("Id can't be null");
        if (string.IsNullOrEmpty(employee.LastName)) throw new InvalidDataException("Last name cannot be empty");
        if (string.IsNullOrEmpty(employee.FirstName)) throw new InvalidDataException("First name cannot be empty");
        if (string.IsNullOrEmpty(employee.UserName)) throw new InvalidDataException("Username cannot be empty");
        if (string.IsNullOrEmpty(employee.Role)) throw new InvalidDataException("Role cannot be empty");
        if (employee.Salary < 0) throw new InvalidDataException("Salary cannot be negative");
        if (string.IsNullOrEmpty(employee.Phone)) throw new InvalidDataException("Phone number cannot be empty");
        if (!Regex.IsMatch(employee.Phone, @"\+\d{1,12}"))
            throw new InvalidDataException("Invalid phone number");
        int age = DateTime.Now.Year - employee.BirthDate.Year;
        if (employee.BirthDate.AddYears(age).ToDateTime(new(0, 0)) > DateTime.Now) --age;
        if (age < 18) throw new InvalidDataException("Worker can't be younger than 18 years old");
        if (string.IsNullOrEmpty(employee.City)) throw new InvalidDataException("City cannot be empty");
        if (string.IsNullOrEmpty(employee.Street)) throw new InvalidDataException("Street cannot be empty");
        if (string.IsNullOrEmpty(employee.ZipCode)) throw new InvalidDataException("Zip code cannot be empty");
        if (!Regex.IsMatch(employee.Phone, @"\d{5}"))
            throw new InvalidDataException("Invalid zip code");

        _employeeRepository.UpdateEmployee(new EmployeeDBModel(
            employee.Id ?? -1,
            employee.LastName,
            employee.FirstName,
            employee.Patronymic,
            employee.Role,
            employee.Salary,
            employee.BirthDate.ToShortDateString(),
            employee.HireDate.ToShortDateString(),
            employee.Phone,
            employee.City,
            employee.Street,
            employee.ZipCode,
            employee.UserName,
            employee.Password));
    }

    public bool AuthenticateEmployee(string username, string password, out long id)
    {
        id = -1;
        
        var employee = _employeeRepository.GetEmployee(username);
        if (employee is null) return false;

        byte[] actualHash = Convert.FromBase64String(employee.Password);
        ReadOnlySpan<byte> salt = actualHash.AsSpan().Slice(HashSize, actualHash.Length - HashSize);
        byte[] passwordHash = HashPassword(password, salt);

        if (!CryptographicOperations.FixedTimeEquals(actualHash, passwordHash)) return false;
        id = employee.Id;
        return true;
    }

    public void DeleteEmployee(long id)
    {
        var employee = _employeeRepository.GetEmployee(id);
        if (employee is null) return;
        _employeeRepository.DeleteEmployee(employee);
    }

    public IEnumerable<EmployeeDTO> GetEmployees()
    {
        foreach (var employee in _employeeRepository.GetEmployees())
        {
            yield return new EmployeeDTO(
                employee.Id,
                $"{employee.Surname} {employee.Name} {employee.Patronymic}",
                employee.Role,
                employee.Salary,
                DateOnly.Parse(employee.DateOfBirth),
                DateOnly.Parse(employee.DateOfStart),
                employee.PhoneNumber,
                $"{employee.City}, {employee.Street}, {employee.ZipCode}");
        }
    }

    public IEnumerable<EmployeeDTO> GetCashiers()
    {
        foreach (var employee in _employeeRepository.GetEmployees(cashiersOnly: true))
        {
            yield return new EmployeeDTO(
                employee.Id,
                $"{employee.Surname} {employee.Name} {employee.Patronymic}",
                employee.Role,
                employee.Salary,
                DateOnly.Parse(employee.DateOfBirth),
                DateOnly.Parse(employee.DateOfStart),
                employee.PhoneNumber,
                $"{employee.City}, {employee.Street}, {employee.ZipCode}");
        }
    }

    public EmployeeDTO? GetEmployee(long id)
    {
        var employee = _employeeRepository.GetEmployee(id);
        if (employee is null) return null;
        return new(
            employee.Id, 
            $"{employee.Surname} {employee.Name} {employee.Patronymic}",
            employee.Role,
            employee.Salary,
            DateOnly.Parse(employee.DateOfBirth),
            DateOnly.Parse(employee.DateOfStart),
            employee.PhoneNumber,
            $"{employee.City}, {employee.Street}, {employee.ZipCode}" );
    }

    public EmployeeDTO? GetEmployee(string lastName)
    {
        var employee = _employeeRepository.GetEmployees().FirstOrDefault(x => x.Surname == lastName);
        if (employee is null) return null;
        return new(
            employee.Id, 
            $"{employee.Surname} {employee.Name} {employee.Patronymic}",
            employee.Role,
            employee.Salary,
            DateOnly.Parse(employee.DateOfBirth),
            DateOnly.Parse(employee.DateOfStart),
            employee.PhoneNumber,
            $"{employee.City}, {employee.Street}, {employee.ZipCode}" );
    }

    public IEnumerable<EmployeeAuthDTO> GetAuthDataOfAll()
    {
        foreach (var employee in _employeeRepository.GetEmployees())
        {
            yield return new EmployeeAuthDTO(employee.Id, employee.Username, employee.Password, employee.Role);
        }
    }

    public EmployeeAuthDTO GetAuthData(long id)
    {
        var employee = _employeeRepository.GetEmployee(id);
        return new EmployeeAuthDTO(employee.Id, employee.Username, employee.Password, employee.Role);
    }

    private static byte[] HashPassword(string password, ReadOnlySpan<byte> salt)
    {
        const int iterations = 600_000;
        
        byte[] result = new byte[HashSize + salt.Length];
        Span<byte> resultHash = result.AsSpan().Slice(0, HashSize);
        Span<byte> resultSalt = result.AsSpan().Slice(HashSize, salt.Length);
        Rfc2898DeriveBytes.Pbkdf2(password, salt, resultHash, iterations, HashAlgorithmName.SHA256);
        salt.CopyTo(resultSalt);
        return result;
    }
}