using DTOModels;

namespace Services.Employee;

public interface IEmployeeService
{
    IEnumerable<EmployeeAuthDTO> GetAuthDataOfAll();
    EmployeeAuthDTO GetAuthData(long id);
    //IEnumerable<EmployeeDto> GetDetailsOfAll();
    //EmployeeDto GetDetails(int id);
}