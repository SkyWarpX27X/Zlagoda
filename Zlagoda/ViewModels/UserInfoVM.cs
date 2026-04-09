using DTOModels;

namespace Zlagoda.ViewModels;

public class UserInfoVM
{
    public EmployeeDTO? CurrentEmployee { get; private set; }

    public void LoadUserInfo()
    {
        // TODO: Implement getting current employee
        // Mock data for now
        CurrentEmployee = new EmployeeDTO(
            12345678,
            "Бердимухамедов Гурбангули Мялікгулийович",
            "Менеджер",
            15000m,
            DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
            "+380501234567",
            "Сараєво, бульвар Героїв Злагоди 4б, 01001"
        );
    }
}
