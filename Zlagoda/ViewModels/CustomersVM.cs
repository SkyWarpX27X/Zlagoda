using DTOModels;
using Services.Customer;

namespace Zlagoda.ViewModels;

public class CustomersVM
{
    private readonly ICustomerService _customerService;
    
    public IEnumerable<CustomerDTO> Customers;
    public bool IsCreating;
    public CustomerModifyDTO? NewCustomer;
    public int? SearchPercent { get; set; }
    
    public IEnumerable<CustomerDTO> FilteredCustomers =>
        SearchPercent.HasValue
            ? _customerService.GetCustomers(SearchPercent.Value)
            : _customerService.GetCustomers();
    
    public CustomersVM(ICustomerService customerService)
    {
        _customerService = customerService;
        Customers = new List<CustomerDTO>();
    }

    public void LoadCustomers()
    {
        Customers = _customerService.GetCustomers();
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
        _customerService.AddCustomer(customer);
        IsCreating = false;
        LoadCustomers();
    }

    public void EditCustomer(CustomerModifyDTO customer)
    {
        _customerService.UpdateCustomer(customer);
        LoadCustomers();
    }

    public void DeleteCustomer(string cardId)
    {
        _customerService.DeleteCustomer(cardId);
        LoadCustomers();
    }
}