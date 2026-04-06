using DBModels;

namespace Repositories.CustomerCard;

public interface ICustomerCardRepository
{
    CustomerCardDBModel? GetCategory(string number);
    IEnumerable<CustomerCardDBModel> GetCustomers(bool sortByName = true, int percent = -1);
    void AddCustomerCard(CustomerCardDBModel card);
    void UpdateCustomerCard(CustomerCardDBModel card);
    void DeleteCustomerCard(CustomerCardDBModel card);
}