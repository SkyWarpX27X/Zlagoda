using DBModels;

namespace Repositories.CustomerCard;

public interface ICustomerCardRepository
{
    CustomerCardDBModel? GetCustomer(string number);
    IEnumerable<CustomerCardDBModel> GetCustomers(bool sortByName = true, int percent = -1);
    IEnumerable<CustomerCardDBModel> GetCustomersBySurname(string surnameQuery);
    void AddCustomerCard(CustomerCardDBModel card);
    void UpdateCustomerCard(CustomerCardDBModel card);
    void DeleteCustomerCard(string number);
}