namespace DTOModels;

public class ProductInStoreDto
{
    public string Upc { get; set; }
    public string? UpcProm { get; set; } //Not sure about this one, do we really need to change barcode on every product with prom?
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsProm { get; set; }

    public ProductInStoreDto(string upc, string? upcProm, string name, decimal price, int quantity, bool isProm)
    {
        Upc = upc;
        UpcProm = upcProm;
        Name = name;
        Price = price;
        Quantity = quantity;
        IsProm = isProm;
    }
}