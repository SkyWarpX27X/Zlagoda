using DBModels;

namespace Repositories.Employee;

public interface IEmployeeRepository
{
    EmployeeDBModel? GetEmployee(long id);
    IEnumerable<EmployeeDBModel> GetEmployees();

}