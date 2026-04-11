using DTOModels;
using Services.Employee;

namespace Zlagoda.ViewModels;

public class UserInfoVM
{
    private readonly IEmployeeService _employeeService;
    public EmployeeDTO? CurrentEmployee { get; private set; }

    public UserInfoVM(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task LoadUserInfo(string username)
    {
        CurrentEmployee = _employeeService.GetEmployee(username);
    }
}
