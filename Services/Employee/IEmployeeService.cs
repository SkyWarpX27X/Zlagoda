using DTOModels;

namespace Services.Employee;

public interface IEmployeeService
{
    void AddEmployee(EmployeeModifyDTO employee);
    void UpdateEmployee(EmployeeModifyDTO employee);
    bool AuthenticateEmployee(string username, string password, out long id);
    void DeleteEmployee(long id);
    
    IEnumerable<EmployeeDTO> GetEmployees();
    IEnumerable<EmployeeDTO> GetCashiers();
    
    EmployeeDTO? GetEmployee(long id);
    EmployeeDTO? GetEmployee(string lastName);
    
    IEnumerable<EmployeeAuthDTO> GetAuthDataOfAll();
    EmployeeAuthDTO GetAuthData(long id);
    //IEnumerable<EmployeeDto> GetDetailsOfAll();
    //EmployeeDto GetDetails(int id);
}