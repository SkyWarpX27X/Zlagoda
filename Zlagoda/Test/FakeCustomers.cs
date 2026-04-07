using DTOModels;

namespace Zlagoda.Test;

static class FakeCustomers
{
    static IEnumerable<CustomerDTO> _customers;
    
    static FakeCustomers()
    {
        _customers = new List<CustomerDTO>()
        {
            new CustomerDTO("1", "Kernes Hennadiy Adolfovych", "+380991112233", "Kharkiv, Knopochna street 10a, 12345", 1),
            new CustomerDTO("2", "Таємний Африкан Свиридович", "101", "MISSINGNO, MISSINGNO, 00000", 5)
        };
    }

    public static IEnumerable<CustomerDTO> GetCustomers()
    {
        return _customers;
    }

}