using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DBModels;
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
            yield return CustomerDbToDto(customer);
        }
    }

    public CustomerDTO? GetCustomer(string cardId)
    {
        var customer = _customerCardRepository.GetCustomer(cardId);
        if (customer is null) return null;
        return CustomerDbToDto(customer);
    }

    public void AddCustomer(CustomerModifyDTO customer)
    {
        ValidateCustomer(customer);
        
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
        ValidateCustomer(customer);
        
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
        _customerCardRepository.DeleteCustomerCard(cardId);
    }

    private CustomerDTO CustomerDbToDto(CustomerCardDBModel customer)
    {
        return new(
            customer.Number,
            $"{customer.Surname} {customer.Name} {customer.Patronymic}",
            customer.PhoneNumber,
            $"{customer.City}, {customer.Street}, {customer.ZipCode}",
            customer.Percent);
    }

    private void ValidateCustomer(CustomerModifyDTO customer)
    {
        if (string.IsNullOrEmpty(customer.LastName)) throw new InvalidDataException("Customer last name is required");
        if (string.IsNullOrEmpty(customer.FirstName)) throw new InvalidDataException("Customer first name is required");
        if (string.IsNullOrEmpty(customer.Phone)) throw new InvalidDataException("Customer phone is required");
        if (!Regex.IsMatch(customer.Phone, @"\+\d{1,12}"))
            throw new InvalidDataException("Invalid phone number");
        if (customer.Percent < 0) throw new InvalidDataException("Customer percent can't be negative");
        if (!string.IsNullOrEmpty(customer.ZipCode) && !Regex.IsMatch(customer.ZipCode, @"\d{5}"))
            throw new InvalidDataException("Invalid zip code");
    }
}