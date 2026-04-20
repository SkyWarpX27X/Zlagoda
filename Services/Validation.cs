using System.Text.RegularExpressions;
using DTOModels;
using Repositories.StoreProduct;

namespace Services;

public static class Validation
{
    public static void ValidateStoreProductCreate(StoreProductModifyDTO storeProduct)
    {
        List<string> errors = new();
        errors.AddRange(ValidateUPC(storeProduct.Upc));
        if (storeProduct.ProductId == 0)
            errors.Add("Product must be selected");
        if (storeProduct.Quantity < 0)
            errors.Add("Quantity can't be negative");
        if (storeProduct.Price <= 0.0m)
            errors.Add("Price must be positive");
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }
    
    public static void ValidateStoreProductUpdate(StoreProductModifyDTO storeProduct)
    {
        List<string> errors = new();
        if (storeProduct.Quantity < 0)
            errors.Add("Quantity can't be negative");
        if (storeProduct.Price < 0.0m)
            errors.Add("Price can't be negative");
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }

    public static void ValidateStoreProductMakePromotional(string promotionalUpc)
    {
        var errors = ValidateUPC(promotionalUpc);
        if (errors.Count > 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
    }

    private static List<string> ValidateUPC(string upc)
    {
        List<string> errors = new();
        if (!Regex.IsMatch(upc, @"\d{12}"))
            errors.Add("Invalid UPC format");
        return errors;
    }
}