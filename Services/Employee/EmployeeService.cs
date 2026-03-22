using DTOModels;
using Repositories.Employee;

namespace Services.Employee;

public class EmployeeService : IEmployeeService
{
    private IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public IEnumerable<EmployeeAuthDto> GetAuthDataOfAll()
    {
        foreach (var employee in _employeeRepository.GetEmployees())
        {
            yield return new EmployeeAuthDto(employee.Id, employee.Username, employee.Password, employee.Role);
        }
    }

    public EmployeeAuthDto GetAuthData(int id)
    {
        var employee = _employeeRepository.GetEmployee(id);
        return new EmployeeAuthDto(employee.Id, employee.Username, employee.Password, employee.Role);
    }
}