using DBModels;

namespace Repositories.Employee;

public interface IEmployeeRepository
{
    EmployeeDBModel? GetEmployee(long id);
    EmployeeDBModel? GetEmployee(string username);
    IEnumerable<EmployeeDBModel> GetEmployees(bool sortBySurname = true, bool cashiersOnly = false);
    // Accepts non-full surname string literals!
    IEnumerable<EmployeeContactInfoDataModel> GetEmployeeContactInfo(string surnameQuery);
    void AddEmployee(EmployeeDBModel employee);
    void UpdateEmployee(EmployeeDBModel employee);
    void DeleteEmployee(EmployeeDBModel employee);
}