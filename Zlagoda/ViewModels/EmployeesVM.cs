using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class EmployeesVM
{
    //TODO switch to real employee service
    //private readonly IEmployeeService _employeeService;

    public IEnumerable<EmployeeDTO> Employees { get; private set; }
    
    public bool IsCreating { get; private set; }
    public EmployeeModifyDTO? NewEmployee { get; private set; }

    public EmployeesVM()
    {
        //_employeeService = employeeService;
        Employees = new List<EmployeeDTO>();
    }

    public void LoadEmployees()
    {
        Employees = FakeEmployees.GetEmployees();
    }
    
    public void ShowCreateNew()
    {
        NewEmployee = new EmployeeModifyDTO(null, "", "", null, "", "", "", 0, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now), "", "", "", "");
        IsCreating = true;
    }

    public void SaveNewEmployee(EmployeeModifyDTO employee)
    {
        IsCreating = false;
        NewEmployee = null;
        // TODO: save employee via service
        LoadEmployees();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewEmployee = null;
    }

    public void EditEmployee(EmployeeModifyDTO employee)
    {
        // TODO: edit employee via service
        LoadEmployees();
    }

    public void DeleteEmployee(string id)
    {
        // TODO: delete employee via service
        LoadEmployees();
    }
}
