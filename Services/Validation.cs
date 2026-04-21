using System.Text.RegularExpressions;
using DTOModels;

namespace Services;

public static class Validation
{
    public static void ValidateCategory(CategoryDTO category)
    {
        List<string> errors = new();
        errors.AddRange(ValidateName(category.Name, 50, "Name"));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateCustomer(CustomerModifyDTO customer)
    {
        List<string> errors = new();
        errors.AddRange(ValidateName(customer.LastName, 50, "Last name"));
        errors.AddRange(ValidateName(customer.FirstName, 50, "First name"));
        if (!string.IsNullOrEmpty(customer.Patronymic))
            errors.AddRange(ValidateName(customer.Patronymic, 50, "Patronymic"));
        errors.AddRange(ValidatePhone(customer.Phone));
        if (customer.Percent < 0) errors.Add("Percent can't be negative.");
        if (!string.IsNullOrEmpty(customer.City))
            errors.AddRange(ValidateName(customer.City, 50, "City"));
        if (!string.IsNullOrEmpty(customer.Street))
            errors.AddRange(ValidateName(customer.Street, 50, "Street"));
        if (!string.IsNullOrEmpty(customer.ZipCode))
            errors.AddRange(ValidateZipCode(customer.ZipCode));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateEmployeeCreate(EmployeeModifyDTO employee)
    {
        List<string> errors = new();
        errors.AddRange(ValidateName(employee.LastName, 50, "Last name"));
        errors.AddRange(ValidateName(employee.FirstName, 50, "First name"));
        if (!string.IsNullOrEmpty(employee.Patronymic))
            errors.AddRange(ValidateName(employee.Patronymic, 50, "Patronymic"));
        errors.AddRange(ValidateName(employee.UserName, 10, "Username"));
        errors.AddRange(ValidateName(employee.Role, 10, "Role"));
        if (employee.Salary <= 0) errors.Add("Salary is required");
        errors.AddRange(ValidatePhone(employee.Phone));
        errors.AddRange(ValidateAge(employee.BirthDate));
        errors.AddRange(ValidateName(employee.City, 50, "City"));
        errors.AddRange(ValidateName(employee.Street, 50, "Street"));
        errors.AddRange(ValidateZipCode(employee.ZipCode));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateEmployeeUpdate(EmployeeModifyDTO employee)
    {
        List<string> errors = new();
        errors.AddRange(ValidateName(employee.LastName, 50, "Last name"));
        errors.AddRange(ValidateName(employee.FirstName, 50, "First name"));
        if (!string.IsNullOrEmpty(employee.Patronymic))
            errors.AddRange(ValidateName(employee.Patronymic, 50, "Patronymic"));
        errors.AddRange(ValidateName(employee.Role, 10, "Role"));
        if (employee.Salary <= 0) errors.Add("Salary is required");
        errors.AddRange(ValidatePhone(employee.Phone));
        errors.AddRange(ValidateAge(employee.BirthDate));
        errors.AddRange(ValidateName(employee.City, 50, "City"));
        errors.AddRange(ValidateName(employee.Street, 50, "Street"));
        errors.AddRange(ValidateZipCode(employee.ZipCode));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateProduct(ProductDTO product)
    {
        List<string> errors = new();
        errors.AddRange(ValidateName(product.Name, 50, "Name"));
        errors.AddRange(ValidateName(product.Category, 50, "Category"));
        errors.AddRange(ValidateName(product.Characteristics, 100, "Characteristics"));
        errors.AddRange(ValidateName(product.Manufacturer, 50, "Manufacturer"));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateReceiptCreate(ReceiptCreateDTO receipt)
    {
        List<string> errors = new();
        foreach (var sale in receipt.Sales)
            errors.AddRange(ValidateSaleCreate(sale));
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateStoreProductCreate(StoreProductModifyDTO storeProduct)
    {
        List<string> errors = new();
        errors.AddRange(ValidateUPC(storeProduct.Upc));
        if (storeProduct.ProductId == 0)
            errors.Add("Product must be selected.");
        if (storeProduct.Quantity < 0)
            errors.Add("Quantity can't be negative.");
        if (storeProduct.Price <= 0.0m)
            errors.Add("Price must be positive.");
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateStoreProductUpdate(StoreProductModifyDTO storeProduct)
    {
        List<string> errors = new();
        if (storeProduct.Quantity < 0)
            errors.Add("Quantity can't be negative.");
        if (storeProduct.Price <= 0.0m)
            errors.Add("Price must be positive.");
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }

    public static void ValidateStoreProductMakePromotional(string promotionalUpc)
    {
        var errors = ValidateUPC(promotionalUpc);
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    private static List<string> ValidateName(string? input, int maxLength, string inputName)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(input)) errors.Add($"{inputName} must be specified.");
        else if (input.Length > maxLength) errors.Add($"{inputName} is too long.");
        return errors;
    }
    
    private static List<string> ValidatePhone(string input)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(input)) errors.Add("Phone number must be specified.");
        else if (!Regex.IsMatch(input, @"\+\d{1,12}")) errors.Add("Invalid phone number format.");
        return errors;
    }

    private static List<string> ValidateAge(DateOnly dateOfBirth)
    {
        List<string> errors = new();
        int age = DateTime.Now.Year - dateOfBirth.Year;
        if (dateOfBirth.AddYears(age).ToDateTime(new(0, 0)) > DateTime.Now) --age;
        if (age < 18) errors.Add("Worker can't be younger than 18 years old.");
        return errors;
    }
    
    private static List<string> ValidateZipCode(string input)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(input)) errors.Add("Zip code must be specified.");
        else if (!Regex.IsMatch(input, @"\d{1,9}")) errors.Add("Invalid zip code format.");
        return errors;
    }
    
    private static List<string> ValidateSaleCreate(SaleCreateDTO sale)
    {
        List<string> errors = new();
        if (string.IsNullOrEmpty(sale.ProductUPC)) errors.Add("Product is required.");
        if (sale.Price <= 0) errors.Add("Price must be positive.");
        if (sale.Quantity < 0) errors.Add("Quantity can't be negative.");
        return errors;
    }

    private static List<string> ValidateUPC(string upc)
    {
        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(upc)) errors.Add("UPC must be specified.");
        else if (!Regex.IsMatch(upc, @"\d{12}")) errors.Add("Invalid UPC format.");
        return errors;
    }
}