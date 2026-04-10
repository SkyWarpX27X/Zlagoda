using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DTOModels;
using Repositories.CustomerCard;

namespace Services.Customer;

public class CustomerService : ICustomerService
{
    private readonly ICustomerCardRepository _customerCardRepository;

    public CustomerService(ICustomerCardRepository customerCardRepository)
    {
        _customerCardRepository = customerCardRepository;
    }

    public IEnumerable<CustomerDTO> GetCustomers(int percent)
    {
        foreach (var customer in _customerCardRepository.GetCustomers(percent: percent))
        {
            yield return new(
                customer.Number,
                $"{customer.Surname} {customer.Name} {customer.Patronymic}",
                customer.PhoneNumber,
                $"{customer.City}, {customer.Street}, {customer.ZipCode}",
                customer.Percent);
        }
    }

    public CustomerDTO? GetCustomer(string cardId)
    {
        var customer = _customerCardRepository.GetCustomer(cardId);
        if (customer is null) return null;
        return new(
            customer.Number,
            $"{customer.Surname} {customer.Name} {customer.Patronymic}",
            customer.PhoneNumber,
            $"{customer.City}, {customer.Street}, {customer.ZipCode}",
            customer.Percent);
    }

    public void AddCustomer(CustomerModifyDTO customer)
    {
        if (string.IsNullOrEmpty(customer.LastName)) throw new InvalidDataException("Customer last name can't be empty");
        if (string.IsNullOrEmpty(customer.FirstName)) throw new InvalidDataException("Customer first name can't be empty");
        if (string.IsNullOrEmpty(customer.Phone)) throw new InvalidDataException("Customer phone can't be empty");
        if (!Regex.IsMatch(customer.Phone, @"\+\d{1,12}"))
            throw new InvalidDataException("Invalid phone number");
        if (customer.Percent < 0) throw new InvalidDataException("Customer percent can't be negative");
        
        var number = new StringBuilder();
        for (int i = 0; i < 10; ++i)
            number.Append($"{RandomNumberGenerator.GetInt32(0, 10)}");
        _customerCardRepository.AddCustomerCard(new(
            number.ToString(), 
            customer.LastName,
            customer.FirstName,
            customer.Patronymic,
            customer.Phone,
            customer.City,
            customer.Street,
            customer.ZipCode,
            customer.Percent));
    }

    public void UpdateCustomer(CustomerModifyDTO customer)
    {
        if (string.IsNullOrEmpty(customer.CardId)) throw new InvalidDataException("CardId can't be empty");
        if (string.IsNullOrEmpty(customer.LastName)) throw new InvalidDataException("Customer last name can't be empty");
        if (string.IsNullOrEmpty(customer.FirstName)) throw new InvalidDataException("Customer first name can't be empty");
        if (string.IsNullOrEmpty(customer.Phone)) throw new InvalidDataException("Customer phone can't be empty");
        if (!Regex.IsMatch(customer.Phone, @"\+\d{1,12}"))
            throw new InvalidDataException("Invalid phone number");
        if (customer.Percent < 0) throw new InvalidDataException("Customer percent can't be negative");
        
        _customerCardRepository.UpdateCustomerCard(new(
            customer.CardId,
            customer.LastName,
            customer.FirstName,
            customer.Patronymic,
            customer.Phone,
            customer.City,
            customer.Street,
            customer.ZipCode,
            customer.Percent));
    }

    public void DeleteCustomer(string cardId)
    {
        var customer = _customerCardRepository.GetCustomer(cardId);
        if (customer is null) return;
        _customerCardRepository.DeleteCustomerCard(customer);
    }
}