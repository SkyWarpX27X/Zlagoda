using DBModels;

namespace Repositories.Employee;

public interface IEmployeeRepository
{
    IEnumerable<EmployeeDb> GetEmployees();
    EmployeeDb GetEmployee(int id);
}