using DBModels;

namespace Repositories.Employee;

public interface IEmployeeRepository
{
    IEnumerable<EmployeeDBModel> GetEmployees();
    EmployeeDBModel GetEmployee(int id);
}