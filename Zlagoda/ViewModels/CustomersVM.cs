using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class CustomersVM
{
    //TODO switch to real customer service
    //private readonly ICustomerService _customerService;
    
    public IEnumerable<CustomerDto> Customers;

    public CustomersVM()
    {
        //_customerService = customerService;
        Customers = new List<CustomerDto>();
    }

    public void LoadCustomers()
    {
        //Customers = _customerService.GetCustomers();
        Customers = FakeCustomers.GetCustomers();
    }
}