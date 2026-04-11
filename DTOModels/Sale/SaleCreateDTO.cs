namespace DTOModels;

public class SaleCreateDTO
{
    public string ProductUPC { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public SaleCreateDTO(string productUPC, decimal price, int quantity)
    {
        ProductUPC = productUPC;
        Price = price;
        Quantity = quantity;
    }
}