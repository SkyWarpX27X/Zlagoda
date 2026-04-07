using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class CustomersVM
{
    //TODO switch to real customer service
    //private readonly ICustomerService _customerService;
    
    public IEnumerable<CustomerDTO> Customers;
    public bool IsCreating;
    public CustomerModifyDTO? NewCustomer;
    
    public CustomersVM()
    {
        //_customerService = customerService;
        Customers = new List<CustomerDTO>();
    }

    public void LoadCustomers()
    {
        //Customers = _customerService.GetCustomers();
        Customers = FakeCustomers.GetCustomers();
    }
    
    public void ShowCreateNew()
    {
        NewCustomer = new CustomerModifyDTO();
        IsCreating = true;
    }
    
    public void CancelCreate()
    {
        IsCreating = false;
        NewCustomer = null;
    }
    
    public void SaveNewCustomer(CustomerModifyDTO customer)
    {
        var foo = 1;
        //_customerService.AddCustomer(customer);
        IsCreating = false;
        LoadCustomers();
    }

    public void EditCustomer(CustomerModifyDTO customer)
    {
        var foo = 1;
        //_customerService.UpdateCustomer(customer);
        LoadCustomers();
    }

    public void DeleteCustomer(string cardId)
    {
        var foo = 1;
        //_customerService.DeleteCustomer(customer);
        LoadCustomers();
    }
}