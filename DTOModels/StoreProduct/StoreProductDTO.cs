namespace DTOModels;

public class StoreProductDTO
{
    public string Upc { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsProm { get; set; }
    public decimal? OldPrice { get; set; }
    
    public StoreProductDTO(string upc, string name, decimal price, int quantity, bool isProm, decimal? oldPrice)
    {
        Upc = upc;
        Name = name;
        Price = price;
        Quantity = quantity;
        IsProm = isProm;
        OldPrice = oldPrice;
    }
}