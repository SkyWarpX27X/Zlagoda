using DTOModels;

namespace Services.Employee;

public interface IEmployeeService
{
    IEnumerable<EmployeeAuthDto> GetAuthDataOfAll();
    EmployeeAuthDto GetAuthData(long id);
    //IEnumerable<EmployeeDto> GetDetailsOfAll();
    //EmployeeDto GetDetails(int id);
}