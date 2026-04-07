using DTOModels;

namespace Services.Customer;

public interface ICustomerService
{
    IEnumerable<CustomerDTO> GetCustomers();
    CustomerDTO GetCustomer(int id);
    void AddCustomer(CustomerModifyDTO customer);
    void UpdateCustomer(CustomerModifyDTO customer);
    void DeleteCustomer(string cardId);
    CustomerModifyDTO ToModifyDTO(CustomerDTO customer);
}