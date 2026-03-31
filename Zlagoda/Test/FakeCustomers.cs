using DTOModels;

namespace Zlagoda.Test;

static class FakeCustomers
{
    static IEnumerable<CustomerDto> _customers;
    
    static FakeCustomers()
    {
        _customers = new List<CustomerDto>()
        {
            new CustomerDto("1", "Kernes Hennadiy Adolfovych", "+380991112233", "Kharkiv Knopochna street 10a 12345", 1),
            new CustomerDto("2", "Таємний Африкан Свиридович", "101", "MISSINGNO MISSINGNO 00000", 5)
        };
    }

    public static IEnumerable<CustomerDto> GetCustomers()
    {
        return _customers;
    }

}