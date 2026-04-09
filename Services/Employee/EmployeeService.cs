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
}