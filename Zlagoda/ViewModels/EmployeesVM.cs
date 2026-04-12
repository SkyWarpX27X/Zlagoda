using DTOModels;
using Services.Employee;

namespace Zlagoda.ViewModels;

public class EmployeesVM
{
    private readonly IEmployeeService _employeeService;

    public IEnumerable<EmployeeDTO> Employees => FilterEmployees();
    
    public bool IsCreating { get; private set; }
    public EmployeeModifyDTO? NewEmployee { get; private set; }
    public string? ErrorMessage { get; private set; }

    public EmployeesVM(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public string SearchSurname { get; set; } = "";
    public string RoleFilter { get; set; } = "All";
    
    public IEnumerable<EmployeeDTO> FilterEmployees()
    {
        if (RoleFilter == "Cashier")
        {
            if (!string.IsNullOrWhiteSpace(SearchSurname))
            {
                return _employeeService.SearchEmployees(SearchSurname, true);
            }
            return _employeeService.GetEmployees(true);
        }
        if (!string.IsNullOrWhiteSpace(SearchSurname))
        {
            return  _employeeService.SearchEmployees(SearchSurname);
        }
        return _employeeService.GetEmployees(false);
    }
    
    public void ShowCreateNew()
    {
        NewEmployee = new EmployeeModifyDTO(null, "", "", null, "", "", "", 0, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now), "", "", "", "");
        IsCreating = true;
        ErrorMessage = null;
    }

    public void SaveNewEmployee(EmployeeModifyDTO employee)
    {
        try
        {
            _employeeService.AddEmployee(employee);
            IsCreating = false;
            NewEmployee = null;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewEmployee = null;
        ErrorMessage = null;
    }
    
    public void ClearError()
    {
        ErrorMessage = null;
    }

    public void EditEmployee(EmployeeModifyDTO employee)
    {
        try
        {
            _employeeService.UpdateEmployee(employee);
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void DeleteEmployee(long id)
    {
        _employeeService.DeleteEmployee(id);
    }
}
