using DBModels;

namespace Repositories.Employee;

public interface IEmployeeRepository
{
    EmployeeDBModel? GetEmployee(long id);
    EmployeeDBModel? GetEmployee(string username);
    IEnumerable<EmployeeDBModel> GetEmployees(bool sortBySurname = true, bool cashiersOnly = false);
    // Accepts non-full surname string literals!
    IEnumerable<EmployeeDBModel> GetEmployeeBySearch(string surnameQuery, bool cashiersOnly = false);
    void AddEmployee(EmployeeDBModel employee);
    void UpdateEmployee(EmployeeDBModel employee);
    void DeleteEmployee(EmployeeDBModel employee);
}