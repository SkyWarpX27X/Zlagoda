using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class EmployeesVM
{
    //TODO switch to real employee service
    //private readonly IEmployeeService _employeeService;

    public IEnumerable<EmployeeDto> Employees;

    public EmployeesVM()
    {
        //_employeeService = employeeService;
        Employees = new List<EmployeeDto>();
    }

    public void LoadEmployees()
    {
        Employees = FakeEmployees.GetEmployees();
    }
}