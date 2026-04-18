using DTOModels;

namespace Services.Customer;

public interface ICustomerService
{
    IEnumerable<CustomerDTO> GetCustomers(int percent = -1, bool sortByName = true);
    CustomerDTO? GetCustomer(string cardId);
    void AddCustomer(CustomerModifyDTO customer);
    void UpdateCustomer(CustomerModifyDTO customer);
    void DeleteCustomer(string cardId);
    IEnumerable<CustomerDTO> SearchCustomers(string query);
}