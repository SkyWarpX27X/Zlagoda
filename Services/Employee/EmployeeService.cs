using System.Security.Cryptography;
using DBModels;
using DTOModels;
using Repositories.Employee;

namespace Services.Employee;

public class EmployeeService : IEmployeeService
{
    private const int HashSize = 32;
    private const int SaltSize = 16;
    
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void AddEmployee(EmployeeModifyDTO employee)
    {
        Validation.ValidateEmployeeCreate(employee);
        
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(salt);
        byte[] passwordHash = HashPassword(employee.Password, salt);
        string password = Convert.ToBase64String(passwordHash);
        
        _employeeRepository.AddEmployee(new EmployeeDBModel(
            employee.LastName,
            employee.FirstName,
            employee.Patronymic,
            employee.Role, employee.Salary,
            employee.BirthDate.ToString("yyyy-MM-dd"),
            employee.HireDate.ToString("yyyy-MM-dd"),
            employee.Phone,
            employee.City,
            employee.Street,
            employee.ZipCode,
            employee.UserName,
            password));
    }

    public void UpdateEmployee(EmployeeModifyDTO employee)
    {
        if (employee.Id is null) throw new InvalidDataException("Id is required");
        Validation.ValidateEmployeeUpdate(employee);

        _employeeRepository.UpdateEmployee(new EmployeeDBModel(
            employee.Id.Value,
            employee.LastName,
            employee.FirstName,
            employee.Patronymic,
            employee.Role,
            employee.Salary,
            employee.BirthDate.ToString("yyyy-MM-dd"),
            employee.HireDate.ToString("yyyy-MM-dd"),
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
        _employeeRepository.DeleteEmployee(id);
    }

    public IEnumerable<EmployeeDTO> GetEmployees(bool cashiersOnly, bool sortBySurname)
    {
        foreach (var employee in _employeeRepository.GetEmployees(sortBySurname, cashiersOnly))
            yield return EmployeeDbToDto(employee);
    }

    public EmployeeDTO? GetEmployee(long id)
    {
        var employee = _employeeRepository.GetEmployee(id)
                       ?? throw new InvalidDataException($"Employee {id} doesn't exist'");
        return EmployeeDbToDto(employee);
    }

    public EmployeeDTO GetEmployee(string username)
    {
        var employee = _employeeRepository.GetEmployee(username)
                       ?? throw new InvalidDataException($"Employee {username} doesn't exist");
        return EmployeeDbToDto(employee);
    }

    public IEnumerable<EmployeeDTO> SearchEmployees(string query, bool cashiersOnly = false)
    {
        foreach (var employee in _employeeRepository.GetEmployeeBySearch(query, cashiersOnly))
            yield return EmployeeDbToDto(employee);
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

    private EmployeeDTO EmployeeDbToDto(EmployeeDBModel employee)
    {
        return new(
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