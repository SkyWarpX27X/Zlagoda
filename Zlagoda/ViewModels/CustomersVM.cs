using DTOModels;
using Services.Customer;

namespace Zlagoda.ViewModels;

public class CustomersVM
{
    private readonly ICustomerService _customerService;
    
    public IEnumerable<CustomerDTO> Customers =>
        SearchPercent.HasValue
            ? _customerService.GetCustomers(SearchPercent.Value)
            : _customerService.GetCustomers();
    public bool IsCreating;
    public CustomerModifyDTO? NewCustomer;
    public int? SearchPercent { get; set; }
    public string? ErrorMessage { get; private set; }
    
    public CustomersVM(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    public void ShowCreateNew()
    {
        NewCustomer = new CustomerModifyDTO();
        IsCreating = true;
        ErrorMessage = null;
    }
    
    public void CancelCreate()
    {
        IsCreating = false;
        NewCustomer = null;
        ErrorMessage = null;
    }
    
    public void ClearError()
    {
        ErrorMessage = null;
    }
    
    public void SaveNewCustomer(CustomerModifyDTO customer)
    {
        try
        {
            _customerService.AddCustomer(customer);
            IsCreating = false;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void EditCustomer(CustomerModifyDTO customer)
    {
        try
        {
            _customerService.UpdateCustomer(customer);
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void DeleteCustomer(string cardId)
    {
        _customerService.DeleteCustomer(cardId);
    }
}