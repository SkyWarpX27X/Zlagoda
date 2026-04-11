using DTOModels;

namespace Services.Employee;

public interface IEmployeeService
{
    void AddEmployee(EmployeeModifyDTO employee);
    void UpdateEmployee(EmployeeModifyDTO employee);
    bool AuthenticateEmployee(string username, string password, out long id);
    void DeleteEmployee(long id);
    
    IEnumerable<EmployeeDTO> GetEmployees(bool cashiersOnly);
    
    EmployeeDTO? GetEmployee(long id);
    EmployeeDTO? GetEmployee(string username);
    IEnumerable<EmployeeDTO> SearchEmployees(string query, bool cashiersOnly = false);
}