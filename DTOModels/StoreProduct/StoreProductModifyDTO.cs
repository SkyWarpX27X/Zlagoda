namespace DTOModels;

public class StoreProductModifyDTO
{
    public string? Upc { get; set; }
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public StoreProductModifyDTO(string? upc, decimal price, int quantity)
    {
        Upc = upc;
        Price = price;
        Quantity = quantity;
    }
}