using DTOModels;

namespace Zlagoda.Test;

static class FakeEmployees
{
    private static IEnumerable<EmployeeDTO> _employees;

    static FakeEmployees()
    {
        _employees = new List<EmployeeDTO>()
        {
            new EmployeeDTO("1", "Світисонечко Мурзік Васильович", "Менеджер", 40000.99m, new DateOnly(2014, 1, 1),
                new DateOnly(2012, 7, 7), "911", "Житомир, проспект Довгоносиків 6г, 32145"),
            new EmployeeDTO("2", "Касирян Касир Касирович", "Касир", 20000, new DateOnly(2026, 1, 10),
                new DateOnly(1999, 10, 5), "+380000000000", "Київ, вулиця Бетонна 99б, 02000")
        };
    }
    
    public static IEnumerable<EmployeeDTO> GetEmployees()
    {
        return _employees;
    }
}